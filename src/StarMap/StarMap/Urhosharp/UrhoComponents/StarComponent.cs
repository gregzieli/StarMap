using Urho;
using Urho.Actions;
using Urho.Shapes;
using Urho.Urho2D;

namespace StarMap.Urhosharp.UrhoComponents
{
    public class StarComponent : StaticSprite2D
    {
        Vector3 _scale;

        public async void Highlight()
        {
            Color = Color.Cyan;
            _scale = Node.Scale;
            // "blinking" animation
            await Node.TryRunActionsAsync(new RepeatForever(new ScaleTo(1, 0.9f), new ScaleTo(1, 2)));
        }

        public async void Dim()
        {
            Color = Color.White;
            Node.RemoveAllActions();
            await Node.TryRunActionsAsync(new EaseBackOut(new ScaleTo(1f, _scale.X, _scale.Y, _scale.Z)));
        }

        public async void Select()
        {
            // For now leave it at that. If there's time, consider using another sprite, something that looks
            // like a target, and have  it rotate around.
            var selectionNode = Node.CreateChild("selection");
            selectionNode.SetScale(0.3f);

            var selection = selectionNode.CreateComponent<Torus>();
            selection.Color = Color.Yellow;
            selectionNode.Rotate(new Quaternion(90, 0, 0));

            // I think that because the action here is RepeatForever, the task is never finished,
            // and so it is impossible to be awaited, if some synchronous work is to be done after.
            // So in RunActionsAsync case, it goes down the line of all the awaits,
            // comes to the last one (here), and while the task performs, returns to the original caller, 
            // which is away from all.
            //
            // But when made async void, it's not awaited anywhere, it should be ok.
            await selectionNode.TryRunActionsAsync(new RepeatForever(new RotateBy(1, 0, 0, 35)));
        }
        public void Deselect()
        {
            // When I had simply RemoveAllChildren,
            // There was a problem: when selecting another star when one is already selected, 
            // the RepeatForever action did not work.
            var a = Node.GetChild("selection");
            a.RemoveAllActions();
            a.RemoveAllComponents();
            Node.RemoveChild(a);
        }
    }
}

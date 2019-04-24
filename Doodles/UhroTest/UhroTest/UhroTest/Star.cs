using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Urho;
using Urho.Actions;
using Urho.Gui;
using Urho.Shapes;

namespace UhroTest
{
  public class Star : Component
  {
    Node _starNode;
    Vector3 _scale;
    Node textNode;
    InputStar _inputStar;
    Text3D text3D;

    public Star(InputStar item)
    {
      _inputStar = item;
      ReceiveSceneUpdates = true;
    }

    public override void OnAttachedToNode(Node node)
    {
      _starNode = node;
      var star = _starNode.CreateComponent<Sphere>();
      star.Color = Color.White;
      _scale = _starNode.Scale;

      textNode = node.CreateChild();
      //textNode.Rotate(new Quaternion(0, 180, 0), TransformSpace.World);
      textNode.Position = new Vector3(0, 2, 0);
      textNode.SetWorldScale(4f);
      text3D = textNode.CreateComponent<Text3D>();
      text3D.SetFont(CoreAssets.Fonts.AnonymousPro, 666);
      text3D.TextEffect = TextEffect.Stroke;

      text3D.Text = _inputStar.Name;
      base.OnAttachedToNode(node);
    }

    public void Deselect()
    {
      _starNode.RemoveAllChildren();
    }

    public async void Select()
    {
      var node = _starNode.CreateChild("torusNode");

      var torus = node.CreateComponent<Torus>();
      torus.Color = Color.Yellow;
      //node.LookAt(Vector3.Up, Vector3.Up);

      node.Rotate(new Quaternion(90, 0, 0));
      await node.RunActionsAsync(new RepeatForever(new RotateBy(1, 30, 0, 0)));
    }

    public void Deselect2()
    {
      _starNode.RemoveAllActions();//TODO: remove only "selection" action
      _starNode.RunActions(new EaseBackOut(new ScaleTo(1f, _scale.X, _scale.Y, _scale.Z)));
    }

    public void Select2()
    {
      // "blinking" animation
      _starNode.RunActions(new RepeatForever(new ScaleTo(1f, 0.5f), new ScaleTo(1f, 1.5f)));
    }
  }
}

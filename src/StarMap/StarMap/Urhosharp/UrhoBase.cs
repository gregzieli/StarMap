using StarMap.Cll.Constants;
using System;
using Urho;
using XF = Xamarin.Forms;

namespace StarMap.Urhosharp
{
    public abstract class UrhoBase : Application
    {
        [Preserve]
        public UrhoBase(ApplicationOptions options) : base(options)
        { }

        protected Scene _scene;
        protected Octree _octree;
        protected Node _lightNode, _cameraNode;

        protected abstract void HandleException(Exception ex);

        protected abstract void FillScene();

        protected override async void Start()
        {
            try
            {
                base.Start();
                CreateScene();
                FillScene();
            }
            catch (Exception e)
            {
                HandleException(e);
                await Exit();
            }
        }

        private void CreateScene()
        {
            _scene = new Scene();
            _octree = _scene.CreateComponent<Octree>();
            _cameraNode = _scene.CreateChild();
            _cameraNode.CreateComponent<Camera>();
            _lightNode = _cameraNode.CreateChild();
            _lightNode.CreateComponent<Light>();

            Renderer.SetViewport(0, new Viewport(Context, _scene, _cameraNode.GetComponent<Camera>(), null));
        }

        protected void PublishError<T>(T payload) where T : Exception
          // Must be main thread, else Android gets an exception "Can't create handler inside thread that has not called Looper.prepare()"
          => XF.Device.BeginInvokeOnMainThread(() => XF.MessagingCenter.Send(payload, MessageKeys.UrhoError));
    }
}

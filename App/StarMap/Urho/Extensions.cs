using Urho;
using Urho.Physics;

namespace StarMap.Urho
{
  public static class Extensions
  {
    public static Node AddCollisionSupport(this Node node, float size)
    {
      var collisionNode = node.CreateChild("collision");
      collisionNode.CreateComponent<RigidBody>();
      var collisionShape = collisionNode.CreateComponent<CollisionShape>();
      collisionShape.SetSphere(size, Vector3.Zero, Quaternion.Identity);

      return node;
    }
  }
}

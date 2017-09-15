using Urho;
using Urho.Physics;

namespace StarMap.Urho
{
  public static class Extensions
  {
    /// <summary>
    /// Adds components to the node introducing it to the PhysicsWorld.
    /// </summary>
    /// <param name="size">Size of the node's <see cref="CollisionShape"/>.</param>
    /// <returns>The node that's been modified.</returns>
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

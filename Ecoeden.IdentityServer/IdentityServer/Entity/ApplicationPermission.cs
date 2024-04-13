namespace IdentityServer.Entity
{
    public class ApplicationPermission
    {
        public ApplicationPermission(string name)
        {
            Id = Guid.NewGuid().ToString();
            Name = name;
        }

        public string Id { get; private init; }
        public string Name { get; set; }

        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}

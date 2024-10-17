namespace IdentityMicroservice.Model
{
    public class ChangePassword
    {
        public string OldPassword { get; set; } = default!;
        public string NewPassword { get; set; } = default!;
        public string token { get; set; } = default!;
    }
}

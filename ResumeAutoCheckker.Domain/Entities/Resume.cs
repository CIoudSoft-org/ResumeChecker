using ResumeAutoCheckker.Domain.Enums;

namespace ResumeAutoCheckker.Domain.Entities
{
    public class Resume
    {
        public long Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string ResumePath {  get; set; }
        public ResumeStatus Status { get; set; } = ResumeStatus.NotResponded;
        public string? Explanation { get; set; }
    }
}

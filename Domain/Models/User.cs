﻿using Microsoft.AspNetCore.Identity;

namespace Domain.Models;

public class User : IdentityUser<Guid>
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public ICollection<UserAnswerOnQuestion> Answers { get; set; } = new List<UserAnswerOnQuestion>();
}

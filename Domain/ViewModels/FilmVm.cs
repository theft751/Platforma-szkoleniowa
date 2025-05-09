﻿namespace Domain.ViewModels;

public class FilmVm
{
    public Guid Id { get; set; }
    public Guid? ImageId { get; set; }
    public string ImageContentType { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Caption { get; set; } = null!;
    public List<QuestionVM> Questions { get; set; } = new();

}

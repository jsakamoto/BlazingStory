using System.ComponentModel.DataAnnotations;

namespace BlazorToDoApp.Models;

public class ToDoItem
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Please input a title for a new to do item.")]
    public string Title { get; set; } = "";

    public bool IsDone { get; set; }
}

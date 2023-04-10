namespace Api.Helpers;

using System.ComponentModel;
using Common.Models;

public class ErrorResult : Result
{
    public ErrorResult() => this.Succeeded = false;

    [DefaultValue(false)]
    public new bool Succeeded { get; set; }
}

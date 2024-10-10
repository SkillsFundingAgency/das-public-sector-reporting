namespace SFA.DAS.PSRService.Web.SubmitActions;

public sealed class SubmitAction
{
    public string SubmitValue { get; }
    public string ControllerName { get; }
    public string ActionName { get; }
    
    public SubmitAction(string submitValue, string controllerName, string actionName)
    {
        SubmitValue = submitValue;
        ControllerName = controllerName;
        ActionName = actionName;
    }
}
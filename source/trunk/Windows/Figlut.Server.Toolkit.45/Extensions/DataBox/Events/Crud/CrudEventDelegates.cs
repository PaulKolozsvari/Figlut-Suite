namespace Figlut.Server.Toolkit.Extensions.DataBox.Events.Crud
{
    #region Delegates

    public delegate void OnBeforeRefreshFromServer(object sender, BeforeRefreshFromServerArgs e);
    public delegate void OnAfterRefreshFromServer(object sender, AfterRefreshFromServerArgs e);

    public delegate void OnBeforeGridRefresh(object sender, BeforeGridRefreshArgs e);
    public delegate void OnAfterGridRefresh(object sender, AfterGridRefreshArgs e);

    public delegate void OnBeforeAddInputControls(object sender, BeforeAddInputControlsArgs e);
    public delegate void OnAfterAddInputControls(object sender, AfterAddInputControlsArgs e);

    public delegate void OnBeforeCrudOperation(object sender, BeforeCrudOperationArgs e);
    public delegate void OnAfterCrudOperation(object sender, AfterCrudOperationArgs e);

    public delegate void OnBeforeSave(object sender, BeforeSaveArgs e);
    public delegate void OnAfterSave(object sender, AfterSaveArgs e);

    #endregion //Delegates
}
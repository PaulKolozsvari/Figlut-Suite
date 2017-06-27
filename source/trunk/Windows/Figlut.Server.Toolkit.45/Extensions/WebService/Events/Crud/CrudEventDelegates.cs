namespace Figlut.Server.Toolkit.Extensions.WebService.Events.Crud
{
    #region Delegates

    public delegate void OnBeforeWebGetSqlTable(object sender, BeforeWebGetSqlTableArgs e);
    public delegate void OnAfterWebGetSqlTable(object sender, AfterWebGetSqlTableArgs e);
    public delegate void OnBeforeWebInvokeSqlTable(object sender, BeforeWebInvokeSqlTableArgs e);
    public delegate void OnAfterWebInvokeSqlTable(object sender, AfterWebInvokeSqlTableArgs e);
    public delegate void OnBeforeWebInvokeSql(object sender, BeforeWebInvokeSqlArgs e);
    public delegate void OnAfterWebInvokeSql(object sender, AfterWebInvokeSqlArgs e);

    #endregion //Delegates
}

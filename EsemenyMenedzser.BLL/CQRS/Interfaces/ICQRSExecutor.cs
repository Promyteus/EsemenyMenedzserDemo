namespace EsemenyMenedzser.BLL.CQRS.Interfaces
{
    public interface ICQRSExecutor
    {
        Task<TResult> ExecuteQueryAsync<TResult>(IQuery<TResult> query);
        Task<TResult> ExecuteCommandAsync<TResult>(ICommand<TResult> command);
    }
}

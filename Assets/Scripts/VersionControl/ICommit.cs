public interface ICommit : IChainLink<ICommit> {
    string GetMessage();

}
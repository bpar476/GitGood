public interface IChainLink<T> {
    void Relink(T parent);
    T GetParent();
}
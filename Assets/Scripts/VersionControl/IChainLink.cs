using System;
public interface IChainLink<T> : IEquatable<IChainLink<T>>{
    void Relink(T parent);
    T GetParent();
}
using System;

public interface IVersion : IChainLink<IVersion> {
    Guid GetId();
}
public interface IMovementProcessor
{
    void SetMovementStall(float time);

    void SetDecellerationStall(float time);

    void SetInputStall(bool state);
}

/// Representa una "tecla" de la contraseña (qué palanca y en qué estado quedó)
public class LeverInputEvent : IGameEvent
{
    private readonly PasswordPuzzle puzzle;
    private readonly int code;     // el valor que empujamos a la cola del puzzle

    public LeverInputEvent(PasswordPuzzle puzzle, int code)
    {
        this.puzzle = puzzle;
        this.code = code;
    }

    public void Execute()
    {
        puzzle.QueueValue(code);
    }
}

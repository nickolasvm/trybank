namespace Trybank.Lib;

public class TrybankLib
{
    public bool Logged;
    public int loggedUser;

    //0 -> Número da conta
    //1 -> Agência
    //2 -> Senha
    //3 -> Saldo
    public int[,] Bank;
    public int registeredAccounts;
    private int maxAccounts = 50;

    public TrybankLib()
    {
        loggedUser = -99;
        registeredAccounts = 0;
        Logged = false;
        Bank = new int[maxAccounts, 4];
    }

    // 1. Construa a funcionalidade de cadastrar novas contas
    public void RegisterAccount(int number, int agency, int pass)
    {

        if (registeredAccounts == 0)
        {
            Bank[0, 0] = number;
            Bank[0, 1] = agency;
            Bank[0, 2] = pass;
            Bank[0, 3] = 0;
            registeredAccounts = 1;

            return;
        }

        try
        {
            for (int i = 0; i < registeredAccounts + 1; i += 1)
            {
                //  Check if account already exists
                if (Bank[i, 0] == number & Bank[i, 1] == agency)
                {
                    throw new ArgumentException("A conta já está sendo usada!");
                }

                //  Check if position is empty
                if (Bank[i, 0] == 0 & Bank[i, 1] == 0)
                {
                    Bank[i, 0] = number;
                    Bank[i, 1] = agency;
                    Bank[i, 2] = pass;
                    Bank[i, 3] = 0;
                }
            }
            registeredAccounts += 1;
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    // 2. Construa a funcionalidade de fazer Login
    public void Login(int number, int agency, int pass)
    {
        try
        {
            if (Logged) throw new AccessViolationException("Usuário já está logado");

            for (int i = 0; i < registeredAccounts; i += 1)
            {
                if (Bank[i, 0] == number & Bank[i, 1] == agency)
                {
                    if (Bank[i, 2] == pass)
                    {
                        Logged = true;
                        loggedUser = i;
                        return;
                    }

                    throw new ArgumentException("Senha incorreta");
                }
            }

            throw new ArgumentException("Agência + Conta não encontrada");
        }
        catch (AccessViolationException ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    // 3. Construa a funcionalidade de fazer Logout
    public void Logout()
    {
        try
        {
            if (!Logged) throw new AccessViolationException("Usuário não está logado");

            Logged = false;
            loggedUser = -99;
        }
        catch (AccessViolationException ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    // 4. Construa a funcionalidade de checar o saldo
    public int CheckBalance()
    {
        try
        {
            if (!Logged) throw new AccessViolationException("Usuário não está logado");

            return Bank[loggedUser, 3];
        }
        catch (AccessViolationException ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    // 5. Construa a funcionalidade de depositar dinheiro
    public void Deposit(int value)
    {
        try
        {
            if (!Logged) throw new AccessViolationException("Usuário não está logado");

            Bank[loggedUser, 3] += value;
        }
        catch (AccessViolationException ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    // 6. Construa a funcionalidade de sacar dinheiro
    public void Withdraw(int value)
    {
        try
        {
            if (!Logged) throw new AccessViolationException("Usuário não está logado");

            if (Bank[loggedUser, 3] < value) throw new InvalidOperationException("Saldo insuficiente");

            Bank[loggedUser, 3] -= value;
        }
        catch (AccessViolationException ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    // 7. Construa a funcionalidade de transferir dinheiro entre contas
    public void Transfer(int destinationNumber, int destinationAgency, int value)
    {
        try
        {
            if (!Logged) throw new AccessViolationException("Usuário não está logado");

            if (Bank[loggedUser, 3] < value) throw new InvalidOperationException("Saldo insuficiente");

            for (int i = 0; i < registeredAccounts; i += 1)
            {
                if (Bank[i, 0] == destinationNumber & Bank[i, 1] == destinationAgency)
                {
                    Bank[loggedUser, 3] -= value;
                    Bank[i, 3] += value;

                    break;
                }
            }
        }
        catch (AccessViolationException ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}

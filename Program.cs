using System.Text;
using DesafioProjetoHospedagem.Models;

namespace DesafioProjetoHospedagem
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            var sistema = new SistemaHospedagem();
            sistema.Executar();
        }
    }

    class SistemaHospedagem
    {
        private List<Pessoa> hospedes = new List<Pessoa>();
        private List<Suite> suites = new List<Suite>();
        private List<Reserva> reservas = new List<Reserva>();

        public void Executar()
        {
            while (true)
            {
                MostrarMenu();
                string opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        CadastrarHospede();
                        break;
                    case "2":
                        CadastrarSuite();
                        break;
                    case "3":
                        CriarReserva();
                        break;
                    case "4":
                        ListarHospedes();
                        break;
                    case "5":
                        ListarSuites();
                        break;
                    case "6":
                        ListarReservas();
                        break;
                    case "0":
                        Console.WriteLine("Saindo do sistema...");
                        return;
                    default:
                        Console.WriteLine("Opção inválida!");
                        break;
                }

                Console.WriteLine("\nPressione qualquer tecla para continuar...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        private void MostrarMenu()
        {
            Console.WriteLine("=== SISTEMA DE HOSPEDAGEM ===");
            Console.WriteLine("1 - Cadastrar Hóspede");
            Console.WriteLine("2 - Cadastrar Suíte");
            Console.WriteLine("3 - Criar Reserva");
            Console.WriteLine("4 - Listar Hóspedes");
            Console.WriteLine("5 - Listar Suítes");
            Console.WriteLine("6 - Listar Reservas");
            Console.WriteLine("0 - Sair");
            Console.Write("Escolha uma opção: ");
        }

        private void CadastrarHospede()
        {
            Console.WriteLine("\n=== CADASTRO DE HÓSPEDE ===");

            Console.Write("Nome: ");
            string nome = Console.ReadLine();

            Console.Write("Sobrenome: ");
            string sobrenome = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(nome))
            {
                Pessoa hospede = new Pessoa(nome, sobrenome);
                hospedes.Add(hospede);
                Console.WriteLine($"Hóspede {hospede.NomeCompleto} cadastrado com sucesso!");
            }
            else
            {
                Console.WriteLine("Nome é obrigatório!");
            }
        }

        private void CadastrarSuite()
        {
            Console.WriteLine("\n=== CADASTRO DE SUÍTE ===");

            Console.Write("Tipo da Suíte: ");
            string tipoSuite = Console.ReadLine();

            Console.Write("Capacidade: ");
            if (int.TryParse(Console.ReadLine(), out int capacidade))
            {
                Console.Write("Valor da Diária: ");
                if (decimal.TryParse(Console.ReadLine(), out decimal valorDiaria))
                {
                    Suite suite = new Suite(tipoSuite, capacidade, valorDiaria);
                    suites.Add(suite);
                    Console.WriteLine($"Suíte {tipoSuite} cadastrada com sucesso!");
                }
                else
                {
                    Console.WriteLine("Valor da diária inválido!");
                }
            }
            else
            {
                Console.WriteLine("Capacidade inválida!");
            }
        }

        private void CriarReserva()
        {
            if (hospedes.Count == 0)
            {
                Console.WriteLine("Não há hóspedes cadastrados!");
                return;
            }

            if (suites.Count == 0)
            {
                Console.WriteLine("Não há suítes cadastradas!");
                return;
            }

            Console.WriteLine("\n=== CRIAR RESERVA ===");

            List<Pessoa> hospedesSelecionados = SelecionarHospedes();
            if (hospedesSelecionados.Count == 0) return;

            Suite suiteSelecionada = SelecionarSuite();
            if (suiteSelecionada == null) return;

            if (hospedesSelecionados.Count > suiteSelecionada.Capacidade)
            {
                Console.WriteLine($"A suíte {suiteSelecionada.TipoSuite} tem capacidade para {suiteSelecionada.Capacidade} hóspedes!");
                return;
            }

            Console.Write("Quantidade de dias: ");
            if (int.TryParse(Console.ReadLine(), out int dias))
            {
                try
                {
                    Reserva reserva = new Reserva(dias);
                    reserva.CadastrarSuite(suiteSelecionada);
                    reserva.CadastrarHospedes(hospedesSelecionados);

                    reservas.Add(reserva);

                    Console.WriteLine("\n=== RESERVA CRIADA COM SUCESSO ===");
                    Console.WriteLine($"Hóspedes: {reserva.ObterQuantidadeHospedes()}");
                    Console.WriteLine($"Suíte: {suiteSelecionada.TipoSuite}");
                    Console.WriteLine($"Dias: {dias}");
                    Console.WriteLine($"Valor Total: R$ {reserva.CalcularValorDiaria():F2}");

                    if (dias >= 10)
                    {
                        Console.WriteLine("Desconto de 10% aplicado para reservas de 10+ dias!");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao criar reserva: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Quantidade de dias inválida!");
            }
        }

        private List<Pessoa> SelecionarHospedes()
        {
            List<Pessoa> selecionados = new List<Pessoa>();

            Console.WriteLine("\nHóspedes disponíveis:");
            for (int i = 0; i < hospedes.Count; i++)
            {
                Console.WriteLine($"{i + 1} - {hospedes[i].NomeCompleto}");
            }

            Console.Write("Digite os números dos hóspedes (separados por vírgula): ");
            string input = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(input))
            {
                string[] indices = input.Split(',');
                foreach (string indice in indices)
                {
                    if (int.TryParse(indice.Trim(), out int i) && i > 0 && i <= hospedes.Count)
                    {
                        selecionados.Add(hospedes[i - 1]);
                    }
                }
            }

            return selecionados;
        }

        private Suite SelecionarSuite()
        {
            Console.WriteLine("\nSuítes disponíveis:");
            for (int i = 0; i < suites.Count; i++)
            {
                Console.WriteLine($"{i + 1} - {suites[i].TipoSuite} (Capacidade: {suites[i].Capacidade}, Valor: R$ {suites[i].ValorDiaria:F2})");
            }

            Console.Write("Escolha o número da suíte: ");
            if (int.TryParse(Console.ReadLine(), out int indice) && indice > 0 && indice <= suites.Count)
            {
                return suites[indice - 1];
            }

            Console.WriteLine("Suíte inválida!");
            return null;
        }

        private void ListarHospedes()
        {
            Console.WriteLine("\n=== HÓSPEDES CADASTRADOS ===");
            if (hospedes.Count == 0)
            {
                Console.WriteLine("Nenhum hóspede cadastrado.");
                return;
            }

            for (int i = 0; i < hospedes.Count; i++)
            {
                Console.WriteLine($"{i + 1} - {hospedes[i].NomeCompleto}");
            }
        }

        private void ListarSuites()
        {
            Console.WriteLine("\n=== SUÍTES CADASTRADAS ===");
            if (suites.Count == 0)
            {
                Console.WriteLine("Nenhuma suíte cadastrada.");
                return;
            }

            for (int i = 0; i < suites.Count; i++)
            {
                Console.WriteLine($"{i + 1} - {suites[i].TipoSuite} (Capacidade: {suites[i].Capacidade}, Valor: R$ {suites[i].ValorDiaria:F2})");
            }
        }

        private void ListarReservas()
        {
            Console.WriteLine("\n=== RESERVAS CADASTRADAS ===");
            if (reservas.Count == 0)
            {
                Console.WriteLine("Nenhuma reserva cadastrada.");
                return;
            }

            for (int i = 0; i < reservas.Count; i++)
            {
                var reserva = reservas[i];
                Console.WriteLine($"\nReserva {i + 1}:");
                Console.WriteLine($"  Hóspedes: {reserva.ObterQuantidadeHospedes()}");
                Console.WriteLine($"  Suíte: {reserva.Suite.TipoSuite}");
                Console.WriteLine($"  Dias: {reserva.DiasReservados}");
                Console.WriteLine($"  Valor Total: R$ {reserva.CalcularValorDiaria():F2}");
            }
        }
    }
}
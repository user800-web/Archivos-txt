using System;
using System.IO;

namespace ManejoArchivos
{
    class Program
    {
        private const string RUTA_ARCHIVO = "./empleados.txt";
        static void Main(string[] args)
        {
            // Verificar si existe el archivo
            if (!File.Exists(RUTA_ARCHIVO))
            {
                // Si no existe, crear el archivo vacío
                File.Create(RUTA_ARCHIVO).Close();
            }

            MenuEmpleados();
        }

        static void MenuEmpleados()
        {
            int opcion = 0;
            do
            {
                Console.WriteLine("MENÚ DE OPCIONES");
                Console.WriteLine("1. Listar los empleados.");
                Console.WriteLine("2. Buscar empleado por cédula.");
                Console.WriteLine("3. Insertar empleado.");
                Console.WriteLine("4. Modificar empleado.");
                Console.WriteLine("5. Eliminar empleado.");
                Console.WriteLine("Digite 0 y presione enter para salir del programa.");

                Console.Write("Seleccione una opción: ");
                while (!int.TryParse(Console.ReadLine(), out opcion))
                {
                    Console.WriteLine("Por favor ingrese una opción válida.");
                    Console.Write("Seleccione una opción: ");

                }

                Console.Clear();

                if (opcion == 1)
                {
                    ListarEmpleados();
                    Console.ReadKey();
                }
                if (opcion == 2)
                {
                    BuscarEmpleado();
                    Console.ReadKey();
                }
                if (opcion == 3)
                {
                    RegistrarEmpleado();
                    Console.ReadKey();
                }
                if (opcion == 4)
                {
                    EditarEmpleado();
                    Console.ReadKey();
                }
                if (opcion == 5)
                {
                    EliminarEmpleado();
                    Console.ReadKey();
                }

                Console.Clear();

            } while (opcion != 0);
        }

        static void ListarEmpleados()
        {
            Console.WriteLine("LISTA DE EMPLEADOS");
            Console.WriteLine("Cédula\tNombre\tCargo\tSalario");

            // Abrir el lector del archivo
            StreamReader reader = new StreamReader(RUTA_ARCHIVO);
            string linea = string.Empty;

            while ((linea = reader.ReadLine()) != null)
            {
                string[] datos = linea.Split('\t');

                if (datos.Length == 4)
                {
                    string cedula = datos[0];
                    string nombre = datos[1];
                    string cargo = datos[2];
                    double salario = double.Parse(datos[3]);

                    Console.WriteLine(cedula + "\t" + nombre + "\t" + cargo + "\t" + salario);
                }
            }

            reader.Close();
        }
        static void RegistrarEmpleado()
        {
            Console.Clear();

            Console.WriteLine("Número de cédula: ");
            string cedula = Console.ReadLine();
            while (!ValidarCedula(cedula) || EmpleadoRepetido(cedula))
            {
                Console.Clear();
                Console.WriteLine("Número de cédula no válido (debe tener 10 caracteres y no debe repetirse), vuelva a ingresarlo: ");
                cedula = Console.ReadLine();
            }

            Console.WriteLine("Nombres: ");
            string nombre = Console.ReadLine();

            Console.WriteLine("Cargo: ");
            string cargo = Console.ReadLine();

            Console.WriteLine("Salario: ");
            double salario = 0;

            // Mientras no sea válido el salario ingresado solicitarlo
            while (!double.TryParse(Console.ReadLine(), out salario))
            {
                Console.WriteLine("Por favor ingrese un salario válido");
            }

            // Abrir el archivo en modo Append para agregar datos al final
            StreamWriter writer = new StreamWriter(RUTA_ARCHIVO, true);
            writer.WriteLine($"{cedula}\t{nombre}\t{cargo}\t{salario}");

            Console.WriteLine("\nEmpleado registrado correctamente");

            writer.Close();
        }
        static void EditarEmpleado()
        {
            Console.Write("Ingrese la cédula del empleado que desea editar: ");
            string cedula = Console.ReadLine();

            StreamReader reader = new StreamReader(RUTA_ARCHIVO);
            string linea = string.Empty;
            bool encontrado = false;

            string lineas_actualizadas = string.Empty;

            while ((linea = reader.ReadLine()) != null)
            {
                string[] datos = linea.Split('\t');

                if (datos.Length == 4 && datos[0] == cedula)
                {
                    Console.WriteLine("Empleado encontrado. Ingrese los nuevos datos:");

                    Console.Write("Nuevo nombre: ");
                    string nuevo_nombre = Console.ReadLine();

                    Console.Write("Nuevo cargo: ");
                    string nuevo_cargo = Console.ReadLine();

                    Console.Write("Nuevo salario: ");
                    double nuevo_salario = 0;

                    // Mientras no sea válido el salario ingresado solicitarlo
                    while (!double.TryParse(Console.ReadLine(), out nuevo_salario))
                    {
                        Console.WriteLine("Por favor ingrese un salario válido");
                        Console.Write("Nuevo salario: ");
                    }


                    // Modificar la línea con los nuevos datos
                    linea = $"{cedula}\t{nuevo_nombre}\t{nuevo_cargo}\t{nuevo_salario}";

                    encontrado = true;
                }

                // Concatenar con un salto de línea, cada registro del archivo
                lineas_actualizadas += linea + "\n";
            }

            reader.Close();

            if (encontrado)
            {
                // Escribir sobre el archivo las nuevas líneas
                StreamWriter writer = new StreamWriter(RUTA_ARCHIVO);
                writer.Write(lineas_actualizadas);
                writer.Close();

                Console.WriteLine("Empleado actualizado correctamente.");
            }
            else
            {
                Console.WriteLine("Empleado no encontrado.");
            }
        }

        static void EliminarEmpleado()
        {
            Console.Write("Ingrese la cédula del empleado que desea eliminar: ");
            string cedula = Console.ReadLine();

            StreamReader reader = new StreamReader(RUTA_ARCHIVO);
            string linea = string.Empty;
            bool encontrado = false;

            string lineas_actualizadas = string.Empty;

            while ((linea = reader.ReadLine()) != null)
            {
                string[] datos = linea.Split('\t');

                if (datos.Length == 4 && datos[0] == cedula)
                {
                    encontrado = true;

                    // Evitar que se concatene el registro que
                    // tiene la cédula búsqueda
                    continue;
                }

                // Concatenar con un salto de línea, cada registro del archivo
                lineas_actualizadas += linea + "\n";
            }

            reader.Close();

            if (encontrado)
            {
                // Escribir sobre el archivo las nuevas líneas
                StreamWriter writer = new StreamWriter(RUTA_ARCHIVO);
                writer.Write(lineas_actualizadas);
                writer.Close();

                Console.WriteLine("Empleado eliminado correctamente.");
            }
            else
            {
                Console.WriteLine("Empleado no encontrado.");
            }
        }

        static string[] BuscarEmpleado()
        {
            string[] empleado = new string[4];

            Console.Write("Ingrese la cédula que desea buscar: ");
            string cedulaBuscar = Console.ReadLine();

            StreamReader reader = new StreamReader(RUTA_ARCHIVO);
            string linea = string.Empty;
            bool encontrado = false;

            while ((linea = reader.ReadLine()) != null)
            {
                string[] datos = linea.Split('\t');

                if (datos.Length == 4)
                {
                    string cedula = datos[0];
                    string nombre = datos[1];
                    string cargo = datos[2];
                    double salario = double.Parse(datos[3]);


                    if (cedula == cedulaBuscar)
                    {
                        Console.WriteLine("Cédula\tNombre\tCargo\tSalario");
                        Console.WriteLine(cedula + "\t" + nombre + "\t" + cargo + "\t" + salario);

                        // Guardar los datos en el arreglo
                        empleado[0] = cedula;
                        empleado[1] = nombre;
                        empleado[2] = cargo;
                        empleado[3] = salario.ToString();

                        encontrado = true;
                        break;
                    }
                }
            }

            if (!encontrado)
            {
                Console.WriteLine("Empleado no encontrado.");
            }

            reader.Close();
            return empleado;
        }

        public static bool EmpleadoRepetido(string cedula_verificar) 
        {
            StreamReader reader = new StreamReader(RUTA_ARCHIVO);
            string linea = string.Empty;
            bool encontrado = false;

            while ((linea = reader.ReadLine()) != null)
            {
                string[] datos = linea.Split('\t');

                if (datos.Length == 4)
                {
                    string cedula = datos[0];
                    string nombre = datos[1];
                    string cargo = datos[2];
                    double salario = double.Parse(datos[3]);


                    if (cedula == cedula_verificar)
                    {                       
                        encontrado = true;
                        break;
                    }
                }
            }

            reader.Close();
            return encontrado;
        }

        public static bool ValidarCedula(string cedula)
        {
            // Verificar que haya texto escrito
            if (String.IsNullOrWhiteSpace(cedula)) return false;

            // Verificar que cuente con 10 caracteres
            if (cedula.Length != 10) return false;

            // Verificar que los 10 caracteres sean dígitos
            for (int i = 0; i < cedula.Length; i++)
            {
                if (!char.IsDigit(cedula[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}

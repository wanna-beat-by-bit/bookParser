# bookParser
Тестовое задание
Цели:
  1. Подготовить базу данных для книг
  2. Создать сервис для парсинга книг с сайта

 # Предлагаемое решение:
Создать сервис на restful архитектуре, выполняющий функции:
  1. Парсинг сайта (При наличии реализованной бизнес логики парсинга заданного сайта)
  2. Вывод имеющейся базы спаршенных книг (Обращение к функции должно содеражть json объект с описанием книг(и) )

# Проблемы на пути к разработке:
1. В основении языка C#
  * ?: Для чего нужны интерфейсы, и как их применять
  * +: Посе поиска информации, и пары примеров, было выяснено, что данная структура данных может
    использоваться как обобщенный тип данных. К примеру, если имеется метод
    ```
    public void output (IList data)(){...}
    ```
    В качестве параметра используется интерфейс структуры данных List, это означает, что на вход
    функции должен быть передан такой объект, которые реализует весь функционал объекта List,
    в качестве которого может быть как сам объект List, так и любая кастомная структура, реализующая
    интерфейс IList. А более общим по отношению структурой будет ICollection, и т.д.
    Также применение интерфейсов играет роль в построении архитектуры. Он может описывать реализуемый 
    функционал слоя, допустим, бизнес-логики приложения.
  
2. В освоении связи С# и БД:
  * ?: Как подключить БД к бизнес логике и контроллеру?
  * +: БД будет рассматриваться по отношению к ним как сервис, который нужно зарегистрировать в систем сервис
	контейнере при помощи команды:
	```
 	services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(builder.ConnectionString));
	```
	а затем добавить интерфейс в область видимости всего проекта, что позволит внедрять БД в любой класс
	с помощью коснтруктора. Добавление в область видимости производится так:
	```
 	services.AddScoped<IRepository>(_ => new Repository(builder.ConnectionString));
	```
	При этом, используется интерфейс, чтобы была возможность использования другой базы данных.

3. В освоении архитектурного решения:
  * ?: Как заставить работать слой контроллера?
  * +: Достаточно зарегистрировать его в классе Startup. Добавлять в область видимости не нужно. Все контроллеры, 
	которые наследуются от BaseController, будут работоспособны после регистрации. 

  * ?: Как устроить архитектуру MVC с применением Dependency Injection?
  * +: Архитектура будет состоять из слоев контроллера для обработки прихоядщих а API запросов,
	далее будет слой бизнес логики, необходимый для реализации фукнкционала парсинга, а также
	будет слой репозитория, который позволит производить операции с БД.
	Слой котроллера, как я видел по модели MVC, может обращаться к БД минуя уровень бизнес логики, поэтому
	в контролллер будет внедрены слои бизнес логики и репозитория.

4. В реализации парсера:
  * ?: Как парсить html документы?
  * +: Есть два варианта. Первый сложный, на который ушло много времени без результата. Это реализовать функции 
	getRequest и postRequest, которые громоздкие в реализации. Далее вопспользоваться сниффером HTTP пакетов,
	и отследить, какие запросы нужно сделать, чтобы добраться до нужной информации(очень сложно разобраться).
	Далее, если получилось установить цепочку post и get запросов, с помощью функций get и post реквестов
	заполучить нужны страницу, и уже отталкиваясь от нее хардкодить индексы нужной информации, в надежде, 
	что программа не сломается при отстутствии каких либо html тэгов. 
	    Второй вариант легче, и получилось его реализовать. Заключается в парсинге html документа, как DOM объекта,
	благодаря чему понятно, как взаимодействовать со страницей. Также это исключает точное вычисление позиций тэгов.

# Как поднять проект на своем компьютере
    Чтобы запустить проект у себя, необходимо выполнить шаги (Все действия производить из главной директории):
1. Поднять докер контейнер для базы данных командой:
    ```
    docker run --name=bookDB -e POSTGRES_PASSWORD='qwerty' -p 5436:5432 -d --rm postgres
    ```
2. Произвести миграцию данных таблиц с помощью утилиты migrate:
    ```
    migrate -path ./schema -database 'postgres://postgres:qwerty@localhost:5436/postgres?sslmode=disable' up
    ```
    Для ее установки под Ubuntu следовать командам:
    * curl -s https://packagecloud.io/install/repositories/golang-migrate/migrate/script.deb.sh | sudo bash
    * sudo apt-get update
    * sudo apt-get install migrate
3. Запуск программы:
    ```
    dotnet run
    ```
     

# Обращение к API
    Для тестирования и разработки API использовалось ПО Postman.
    Исходя из реализованного функционала, имеются следующие запросы:
1. Запрос на парсинг isbn номеров списка разрешенных книг:
    GET запрос
    localhost:5000/api/getAllowedBooks/{amount}
    Данный запрос позволяет получить массив json isbn номеров 
    разрешенных книг. Также сервер делает локальную копию у себя
    для дальнейшего использования.
    

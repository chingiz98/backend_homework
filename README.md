# BackendHomework

#### Код для создания БД
    CREATE DATABASE test;
#### Код для создания таблиц
    CREATE EXTENSION IF NOT EXISTS "uuid-ossp";
    CREATE TABLE users (
    	"id" uuid NOT NULL DEFAULT uuid_generate_v4(),
     	"username" text NOT NULL,
     	"password" text NOT NULL,
     	"name" text NOT NULL,
     	"status" text NOT NULL,
    	PRIMARY KEY (id),
    	UNIQUE(username)
    );
    CREATE TABLE accounts (
    	"id" bigserial NOT NULL,
    	"owner_id" uuid NOT NULL,
    	"name" text DEFAULT 'Unnamed',
    	"amount" numeric NOT NULL DEFAULT 0.0,
    	"closed" boolean NOT NULL DEFAULT FALSE,
    	PRIMARY KEY (id),
    	FOREIGN KEY (owner_id) REFERENCES users(id) ON DELETE CASCADE
    );
    ALTER SEQUENCE accounts_id_seq RESTART WITH 4000000001 INCREMENT BY 1;
    CREATE TABLE transactions (
    	"to_id" bigint NOT NULL,
    	"from_id" bigint,
    	"amount" numeric NOT NULL,
    	"comment" text,
    	"type" text,
    	"timestamp" timestamp NOT NULL,
    	FOREIGN KEY (to_id) REFERENCES accounts(id)
    );
#### Имя пользователя БД: postgres
#### Пароль БД: 1
# Описание API
## Регистрация 
SignUp [HttpPost("/auth/signup")]

Body:
- username - логин пользователя (в данном случае - e-mail)
- password - пароль
- name - имя пользователя

Пример вызова:
 ```
POST https://localhost:5001/auth/signup
Content-Type: application/json
{
    "username": "test@email.ru",
    "password": "qwerty12345",
    "name": "TestName"
}
```
##### Ответ:
```json
{
 "access_token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjNkZWI2MWQwLTFiNTItNDliZi1iYWY3LTJmMTQxNDg3Yzk2OCIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6Im1vZGVyYXRpb24iLCJuYmYiOjE1ODkwMTcxNjksImV4cCI6MTU4OTAxOTg2OSwiaXNzIjoiQmFja2VuZFNlcnZlciIsImF1ZCI6IkJhY2tlbmRDbGllbnQifQ.KjTzHASevBrFu68ZQDIK5bNLzxohov_O2Myuv1_YuNw",
 "expiration": "2020-05-09T10:24:29Z"
}
```

## Авторизация 
LogIn [HttpPost("/auth/signup")]

Body:
- username - логин пользователя (в данном случае - e-mail)
- password - пароль

Пример вызова:
```
POST https://localhost:5001/auth/login
Content-Type: application/json
    
{
 "username": "test@email.ru",
 "password": "qwerty12345"
}
```
##### Ответ:
```json
{
 "access_token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjNkZWI2MWQwLTFiNTItNDliZi1iYWY3LTJmMTQxNDg3Yzk2OCIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6Im1vZGVyYXRpb24iLCJuYmYiOjE1ODkwMTgwMTIsImV4cCI6MTU4OTAyMDcxMiwiaXNzIjoiQmFja2VuZFNlcnZlciIsImF1ZCI6IkJhY2tlbmRDbGllbnQifQ.GJOe5l33dvDfzRE7v3g3uMsrnGZSSTb8gr7Snafk2Yo",
 "expiration": "2020-05-09T10:38:32Z"
}
```

## Изменить данные пользователя 
UpdateInfo [HttpPost("/user/updateInfo")]

- Функция использует очередь сообщений

Parameters:
- username - новый логин пользователя (в данном случае - e-mail)
- name - новое имя пользователя

Пример вызова:
```
POST https://localhost:5001/user/updateInfo?username=newtest1@mail.com&name=newName
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjNkZWI2MWQwLTFiNTItNDliZi1iYWY3LTJmMTQxNDg3Yzk2OCIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6Im1vZGVyYXRpb24iLCJuYmYiOjE1ODkwMjI2NjUsImV4cCI6MTU4OTAyNTM2NSwiaXNzIjoiQmFja2VuZFNlcnZlciIsImF1ZCI6IkJhY2tlbmRDbGllbnQifQ.gVDx8tDcCyr_sLUln6sXFTv831ow2FwqLFMo6W2SuGk
```
##### Ответ:
```json
{
  "result": "ok"
}
```

## Удалить профиль пользователя 
DeleteProfile [HttpDelete("/user/deleteProfile")]

Пример вызова:
```
DELETE https://localhost:5001/user/deleteProfile
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjNkZWI2MWQwLTFiNTItNDliZi1iYWY3LTJmMTQxNDg3Yzk2OCIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6Im1vZGVyYXRpb24iLCJuYmYiOjE1ODkwMjI2NjUsImV4cCI6MTU4OTAyNTM2NSwiaXNzIjoiQmFja2VuZFNlcnZlciIsImF1ZCI6IkJhY2tlbmRDbGllbnQifQ.gVDx8tDcCyr_sLUln6sXFTv831ow2FwqLFMo6W2SuGk
```
##### Ответ:
```json
{
  "result": "ok"
}
```

## Открыть новый счет 
Create [HttpPost("/accounts/create")]

Parameters:
- name - имя для нового счета

Пример вызова:
```
POST https://localhost:5001/accounts/create?name=NewAccount
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjNkZWI2MWQwLTFiNTItNDliZi1iYWY3LTJmMTQxNDg3Yzk2OCIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6Im1vZGVyYXRpb24iLCJuYmYiOjE1ODkwMjI2NjUsImV4cCI6MTU4OTAyNTM2NSwiaXNzIjoiQmFja2VuZFNlcnZlciIsImF1ZCI6IkJhY2tlbmRDbGllbnQifQ.gVDx8tDcCyr_sLUln6sXFTv831ow2FwqLFMo6W2SuGk
```
##### Ответ:
```json
{
  "id": 4000000002,
  "owner_id": "3deb61d0-1b52-49bf-baf7-2f141487c968",
  "name": "NewAccount",
  "amount": 0.0,
  "closed": false
}
```

## Закрыть счет 
Close [HttpPost("/accounts/close")]

Parameters:
- id - номер счета, который нужно закрыть

Пример вызова:
```
POST https://localhost:5001/accounts/close?id=4000000002
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjNkZWI2MWQwLTFiNTItNDliZi1iYWY3LTJmMTQxNDg3Yzk2OCIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6Im1vZGVyYXRpb24iLCJuYmYiOjE1ODkwMzA0NDQsImV4cCI6MTU4OTAzMzE0NCwiaXNzIjoiQmFja2VuZFNlcnZlciIsImF1ZCI6IkJhY2tlbmRDbGllbnQifQ.QkuBwoN_yI7mFlkAoApzw4fFmhctitPy6DUrknxk9Ms
```
##### Ответ:
```json
{
  "id": 4000000002,
  "owner_id": "3deb61d0-1b52-49bf-baf7-2f141487c968",
  "name": "NewAccount",
  "amount": 0.0,
  "closed": true
}
```

## Переименовать счет 
Rename [HttpPost("/accounts/rename")]

- Функция использует очередь сообщений

Parameters:
- accountId - номер счета, который нужно переименовать
- name - новое имя счета

Пример вызова:
```
POST https://localhost:5001/accounts/rename?accountId=4000000002&name=NewNameForAccount
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjNkZWI2MWQwLTFiNTItNDliZi1iYWY3LTJmMTQxNDg3Yzk2OCIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6Im1vZGVyYXRpb24iLCJuYmYiOjE1ODkwMzA0NDQsImV4cCI6MTU4OTAzMzE0NCwiaXNzIjoiQmFja2VuZFNlcnZlciIsImF1ZCI6IkJhY2tlbmRDbGllbnQifQ.QkuBwoN_yI7mFlkAoApzw4fFmhctitPy6DUrknxk9Ms
```
##### Ответ:
```json
{
  "result": "ok"
}
```

## Получить счета пользователя 
GetAccounts [HttpGet("/accounts/getAccounts")]

Пример вызова:
```
GET https://localhost:5001/accounts/getAccounts
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjNkZWI2MWQwLTFiNTItNDliZi1iYWY3LTJmMTQxNDg3Yzk2OCIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6Im1vZGVyYXRpb24iLCJuYmYiOjE1ODkwMzA0NDQsImV4cCI6MTU4OTAzMzE0NCwiaXNzIjoiQmFja2VuZFNlcnZlciIsImF1ZCI6IkJhY2tlbmRDbGllbnQifQ.QkuBwoN_yI7mFlkAoApzw4fFmhctitPy6DUrknxk9Ms
```
##### Ответ:
```json
[
  {
    "id": 4000000002,
    "owner_id": "3deb61d0-1b52-49bf-baf7-2f141487c968",
    "name": "NewNameForAccount",
    "amount": 0.0,
    "closed": false
  },
  {
    "id": 4000000003,
    "owner_id": "3deb61d0-1b52-49bf-baf7-2f141487c968",
    "name": "AccountNumber2",
    "amount": 0.0,
    "closed": false
  }
]
```

## Пополнить счет 
Deposit [HttpPost("/accounts/deposit")]

- Функция использует очередь сообщений

Parameters:
- id - номер счета, который нужно пополнить
- amount - сумма пополнения 

Пример вызова:
```
POST https://localhost:5001/accounts/deposit?id=4000000003&amount=2500
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjNkZWI2MWQwLTFiNTItNDliZi1iYWY3LTJmMTQxNDg3Yzk2OCIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6Im1vZGVyYXRpb24iLCJuYmYiOjE1ODkwMzA0NDQsImV4cCI6MTU4OTAzMzE0NCwiaXNzIjoiQmFja2VuZFNlcnZlciIsImF1ZCI6IkJhY2tlbmRDbGllbnQifQ.QkuBwoN_yI7mFlkAoApzw4fFmhctitPy6DUrknxk9Ms
```
##### Ответ:
```json
{
  "result": "ok"
}
```

## Выполнить перевод между счетами 
MakeTransaction [HttpPost("/accounts/makeTransaction")]

Parameters:
- fromAccountId - номер счета с которого нужно перевести
- toAccountId - номер счета на который нужно перевести
- amount - сумма
- comment - комментарий к переводу

Пример вызова:
```
POST https://localhost:5001/accounts/makeTransaction?fromAccountId=4000000003&toAccountId=4000000002&amount=350&comment=testComment
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjNkZWI2MWQwLTFiNTItNDliZi1iYWY3LTJmMTQxNDg3Yzk2OCIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6Im1vZGVyYXRpb24iLCJuYmYiOjE1ODkwMzA0NDQsImV4cCI6MTU4OTAzMzE0NCwiaXNzIjoiQmFja2VuZFNlcnZlciIsImF1ZCI6IkJhY2tlbmRDbGllbnQifQ.QkuBwoN_yI7mFlkAoApzw4fFmhctitPy6DUrknxk9Ms
```
##### Ответ:
```json
{
  "result": "ok"
}
```

## Получить историю операций по счету 
GetTransactions [HttpGet("/accounts/getTransactions")]

Parameters:
- accountId - номер счета для которого нужно получить историю операций


Пример вызова:
```
GET https://localhost:5001/accounts/getTransactions?accountId=4000000003
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjNkZWI2MWQwLTFiNTItNDliZi1iYWY3LTJmMTQxNDg3Yzk2OCIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6Im1vZGVyYXRpb24iLCJuYmYiOjE1ODkwMzA0NDQsImV4cCI6MTU4OTAzMzE0NCwiaXNzIjoiQmFja2VuZFNlcnZlciIsImF1ZCI6IkJhY2tlbmRDbGllbnQifQ.QkuBwoN_yI7mFlkAoApzw4fFmhctitPy6DUrknxk9Ms
```
##### Ответ:
```json
[
  {
    "to_id": 4000000003,
    "from_id": 0,
    "amount": 2500,
    "comment": "",
    "type": "deposit",
    "timestamp": "2020-05-09T18:30:49.07208"
  },
  {
    "to_id": 4000000003,
    "from_id": 0,
    "amount": 300,
    "comment": "",
    "type": "deposit",
    "timestamp": "2020-05-09T18:31:00.752201"
  },
  {
    "to_id": 4000000003,
    "from_id": 0,
    "amount": 100,
    "comment": "",
    "type": "deposit",
    "timestamp": "2020-05-09T18:31:21.524958"
  },
  {
    "to_id": 4000000003,
    "from_id": 4000000002,
    "amount": 350,
    "comment": "testComment",
    "type": "transaction",
    "timestamp": "2020-05-09T18:34:20.262418"
  }
]
```



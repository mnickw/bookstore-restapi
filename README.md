# BookStore

Проект в разработке! Вся работа в ветке dev.

## Задача

Создать WebApi для книжного магазина.
Этот WebApi должен имитировать настоящий книжный онлайн-магазин - в нем должна быть возможность просматривать книги, добавлять новые, покупать их, просматривать авторов и т.д. - нет необходимости реализовывать все возможные взаимодействия внутри книжного магазина, но основные должны присутствовать.

## Что сейчас реализовано (ветка dev)

/books
| Api verb (* - authorized) | Description           | Permissions    |
|---------------------------|-----------------------|----------------|
| get                       | get all books         |                |
| post* {...}               | add one book to books | /change:books/ |

/books/{bookId}
| Api verb (* - authorized) | Description | Permissions    |
|---------------------------|-------------|----------------|
| get                       | get book    |                |
| put*                      | change book | /change:books/ |
| delete*                   | delete book | /change:books/ |

/members/{userID}/cart
| Api verb (* - authorized) | Description          | Permissions                        |
|---------------------------|----------------------|------------------------------------|
| get*                      | get user's own cart  | (userID in url == userID in Token) |
| put* {bookId, qnty}       | add one book to cart | (userID in url == userID in Token) |

/orders
| Api verb (* - authorized) | Description                                | Permissions |
|---------------------------|--------------------------------------------|-------------|
| post* пустой              | add order from cart of userID (from Token) | authorized  |

/orders/{orderID}
| Api verb (* - authorized) | Description   | Permissions                          |
|---------------------------|---------------|--------------------------------------|
| get*                      | get own order | (userID in order == userID in Token) |

Добавлена jwt bearer аутентификация. Используется сервис Auth0, но можно позже заменить на свой identity server. Апишки защищены by permissions, которые можно получить в токене.
Добавлен фронтенд в виде ASP.NET Core MVC приложения для тестирования всего процесса аутентификации пользователя. Фронтенд использует OpenID Connect аутентификацию с помощью сервиса Auth0. Было решено воспользоваться возможностью RBAC сервиса Auth0. Теперь в id token встраиваются роли, которые связаны логически с permissions в access токене. (Подробнее опишу этот процесс в следующих коммитах)
tl;dr: теперь пользователь может зайти на сайт (фронтенд), зарегистрироваться и войти, и, если у него есть доступ, он увидит кнопку удалить книгу (кнопки отображаются, если у пользователя есть роль "Moderator", роли пока присваиваются только разработчиком), нажмет на кнопку и вызовет апишку (бэкенд, здесь проверяются есть ли у пользователя разрешения change:books, Auth0 связывает роли с набором разрешений)

## Что нужно сейчас реализовывать

добавить repository pattern там, где нужно (будет полезен для моков)

подумать еще раз о dto моделях

end-to-end тесты для апи

юнит тесты

исправление ошибок, связанных с отправкой неправильных данных

swagger

логирование

фильтрация книг ?authors="name name2"

/members/{userID}/cart/{bookID} - get put delete

## Что нужно реализовывать в принципе

логику с refresh токеном

фронтенд

просмотр своего профиля

более подробное описание всего решения

## Что хочется в перспективе

изменение своего профиля

admin api (as 2nd api?)

/books post* {[...]} - add several books /change:books/

pagination: books (заменить обычный get), user's orders

/orders?userID get с фильтрацией все по своему userID /(userID in url == userID in Token)/

sign up btn

limit for cart

hal и links в апи

shipment, payment, дискаунты/скидки

blazor front как 3rd party app

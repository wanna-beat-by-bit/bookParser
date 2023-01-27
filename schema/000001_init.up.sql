
CREATE TABLE book
(
    id serial not null unique,
    name varchar(128) not null,
    year int not null,
    description text not null unique,
    isbn varchar(16) not null,
    pages int not null,
    imagePath varchar(128) not null
);

CREATE TABLE author
(
    id serial not null unique,
    name varchar(64) not null
);

CREATE TABLE bookToAuthor
(
    id serial not null unique,
    bookId int references book (id) on delete cascade not null,
    authorId int references author (id) on delete cascade not null
);

CREATE TABLE tag
(
    id serial not null unique,
    name varchar(64) not null
);

CREATE TABLE bookToTag
(
    id serial not null unique,
    bookId int references book (id) on delete cascade not null,
    tagId int references tag (id) on delete cascade not null
);

CREATE TABLE genre
(
    id serial not null unique,
    name varchar(64) not null
);

CREATE TABLE bookToGenre
(
    id serial not null unique,
    bookId int references book (id) on delete cascade not null,
    genreId int references genre (id) on delete cascade not null
);
-- Ejecutar en una base nueva, sin estas tablas.


CREATE TABLE usuario (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    username varchar(255),
    mail varchar(255),
    isActive bool,
    password varchar(255)
);

CREATE TABLE login_log (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    usuario_id int,
    fecha timestamp,
    success bool,
    CONSTRAINT login_log_usuario_FK FOREIGN KEY (usuario_id) REFERENCES usuario(id)
);

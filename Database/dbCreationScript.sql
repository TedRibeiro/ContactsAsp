--Cria��o do Banco de Dados 'contacts'
IF NOT EXISTS (SELECT 1
               FROM   sysdatabases
               WHERE  name = N'Contact')
  BEGIN
      CREATE DATABASE Contacts

      PRINT 'Banco de dados "Contacts" criado.';
  END

go

USE Contacts;

--Cria��o da tabela 'Contact'
IF NOT EXISTS (SELECT 1
               FROM   INFORMATION_SCHEMA.TABLES
               WHERE  TABLE_NAME = N'Contact')
  BEGIN
      CREATE TABLE Contact
        (
           ContactId          INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
           ContactName        VARCHAR(255) NOT NULL,
           ContactDateOfBirth DATETIME NOT NULL,
        );

      --Cria��o de �ndice para prevenir duplicatas no nome
      CREATE UNIQUE INDEX IDX_contact_name
        ON Contact (ContactName);

      PRINT 'Tabela "Contact" criada.'
  END

--Cria��o da tabela 'PhoneType'
IF NOT EXISTS (SELECT 1
               FROM   INFORMATION_SCHEMA.TABLES
               WHERE  TABLE_NAME = N'PhoneType')
  BEGIN
      CREATE TABLE PhoneType
        (
           PhoneTypeId   INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
           PhoneTypeName VARCHAR(255) NOT NULL,
        );

      PRINT 'Tabela "PhoneType" criada.';

      --Populando tabela
      INSERT INTO PhoneType
      VALUES      ('Celular'),
                  ('Residencial'),
                  ('Comercial'),
                  ('Fax'),
                  ('Outros');
  END

--Cria��o da tabela 'EmailType'
IF NOT EXISTS (SELECT 1
               FROM   INFORMATION_SCHEMA.TABLES
               WHERE  TABLE_NAME = N'EmailType')
  BEGIN
      CREATE TABLE EmailType
        (
           EmailTypeId   INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
           EmailTypeName VARCHAR(255) NOT NULL,
        );

      PRINT 'Tabela "EmailType" criada.';

      --Populando tabela
      INSERT INTO EmailType
      VALUES      ('Pessoal'),
                  ('Profissional');
  END

--Cria��o da tabela 'Phone'
IF NOT EXISTS (SELECT 1
               FROM   INFORMATION_SCHEMA.TABLES
               WHERE  TABLE_NAME = N'Phone')
  BEGIN
      CREATE TABLE Phone
        (
           PhoneId     INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
           PhoneNumber CHAR(11) NOT NULL,
           PhoneTypeId INT NOT NULL,
           ContactId   INT NOT NULL,
           CONSTRAINT fk_phone_contactid FOREIGN KEY (ContactId) REFERENCES Contact(ContactId) ON DELETE CASCADE,
           CONSTRAINT fk_phone_phonetype FOREIGN KEY (PhoneTypeId) REFERENCES PhoneType(PhoneTypeId)
        );

      PRINT 'Tabela "Phone" criada.';
  END

--Cria��o da tabela 'Email'
IF NOT EXISTS (SELECT 1
               FROM   INFORMATION_SCHEMA.TABLES
               WHERE  TABLE_NAME = N'Email')
  BEGIN
      CREATE TABLE Email
        (
           EmailId      INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
           ContactId    INT NOT NULL,
           EmailAddress VARCHAR(255) NOT NULL,
           EmailTypeId  INT NOT NULL
           CONSTRAINT fk_email_contactid FOREIGN KEY (ContactId) REFERENCES Contact(ContactId) ON DELETE CASCADE,
           CONSTRAINT fk_email_emailtype FOREIGN KEY (EmailTypeId) REFERENCES EmailType(EmailTypeId)
        );

      --Cria��o de �ndice para prevenir duplicatas no tipo de email
      CREATE UNIQUE INDEX IDX_email_contact
        ON Email (ContactId, EmailTypeId);

      PRINT 'Tabela "Email" criada.';
  END 

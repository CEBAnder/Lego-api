CREATE TABLE dbo.Themes (
	theme_id INT PRIMARY KEY,
	[name] VARCHAR(256) NOT NULL,
	[parent_id] INT NULL,
	CONSTRAINT FK_parent_id_theme_id FOREIGN KEY (parent_id) REFERENCES dbo.[Themes]([theme_id])
);

CREATE TABLE dbo.[Sets] (
    set_id INT PRIMARY KEY IDENTITY (1, 1),
	[set_num] VARCHAR(20) NOT NULL,
    [name] VARCHAR (256) NOT NULL,
    [year] int NULL,
    [theme_id] INT NULL,
    [num_parts] INT NULL,
	FOREIGN KEY ([theme_id]) REFERENCES dbo.Themes ([theme_id])
);
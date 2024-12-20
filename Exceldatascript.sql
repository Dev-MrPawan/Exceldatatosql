USE [Exceldata]
GO
/****** Object:  UserDefinedTableType [dbo].[excel]    Script Date: 19-12-2024 18:32:05 ******/
CREATE TYPE [dbo].[excel] AS TABLE(
	[name] [nvarchar](100) NULL,
	[age] [nvarchar](100) NULL,
	[email] [nvarchar](100) NULL
)
GO
/****** Object:  Table [dbo].[excledate]    Script Date: 19-12-2024 18:32:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[excledate](
	[indexid] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](100) NULL,
	[age] [nvarchar](100) NULL,
	[email] [nvarchar](200) NULL,
	[tdate] [date] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbluserinfo]    Script Date: 19-12-2024 18:32:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbluserinfo](
	[indexid] [int] IDENTITY(1000,1) NOT NULL,
	[username] [nvarchar](100) NULL,
	[Password] [nvarchar](100) NULL,
	[Role] [nvarchar](100) NULL,
	[tdate] [nvarchar](100) NULL,
	[email] [nvarchar](100) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[uploaderdetails]    Script Date: 19-12-2024 18:32:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[uploaderdetails](
	[indexid] [int] IDENTITY(500,1) NOT NULL,
	[username] [nvarchar](100) NULL,
	[filename] [nvarchar](1000) NULL,
	[tdate] [date] NULL
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[excledate] ON 

INSERT [dbo].[excledate] ([indexid], [name], [age], [email], [tdate]) VALUES (1, N'Jugnu', N'55', N'j@gmail.com', CAST(N'2024-12-19' AS Date))
INSERT [dbo].[excledate] ([indexid], [name], [age], [email], [tdate]) VALUES (2, N'kumar', N'67', N'Kumar@gmail.com', CAST(N'2024-12-19' AS Date))
INSERT [dbo].[excledate] ([indexid], [name], [age], [email], [tdate]) VALUES (3, N'Pagal', N'98', N'p@gmail.com', CAST(N'2024-12-19' AS Date))
SET IDENTITY_INSERT [dbo].[excledate] OFF
GO
SET IDENTITY_INSERT [dbo].[tbluserinfo] ON 

INSERT [dbo].[tbluserinfo] ([indexid], [username], [Password], [Role], [tdate], [email]) VALUES (1000, N'Admin', N'1234', N'Admin', N'Dec 19 2024  3:50PM', N'pagal@gmail.com')
SET IDENTITY_INSERT [dbo].[tbluserinfo] OFF
GO
SET IDENTITY_INSERT [dbo].[uploaderdetails] ON 

INSERT [dbo].[uploaderdetails] ([indexid], [username], [filename], [tdate]) VALUES (500, N'pagal@gmail.com', N'c88a3682-61e1-434e-b2ee-2dc7d84a5b50_ec.xlsx', CAST(N'2024-12-19' AS Date))
SET IDENTITY_INSERT [dbo].[uploaderdetails] OFF
GO
/****** Object:  StoredProcedure [dbo].[proAdmin]    Script Date: 19-12-2024 18:32:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create procedure [dbo].[proAdmin]               
(         
@phone nvarchar(100)=null,        
@email nvarchar(100)=null,              
@password nvarchar(100)=null                
)                  
as                  
Begin                    
 if exists (select * from tbluserinfo where email=@email and password = @password)                    
begin                    
select '1'                  
select * from tbluserinfo where email=@email and password = @password       
end                    
else                  
begin              
select '0'                  
end                  
End 
GO
/****** Object:  StoredProcedure [dbo].[Proinfo]    Script Date: 19-12-2024 18:32:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[Proinfo](
@action nvarchar(100)
)
as begin
if(@action='up')
begin
select * from uploaderdetails order by indexid desc
end

if(@action='ex')
begin
select * from excledate order by indexid desc
end




end
GO
/****** Object:  StoredProcedure [dbo].[proupload]    Script Date: 19-12-2024 18:32:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[proupload](
@username nvarchar(100)=null, 	
@filename nvarchar(100)=null,	
@ex excel readonly 
)
as
begin
insert into uploaderdetails(username	,filename	,tdate)
values(@username,@filename,getdate())

insert into excledate(name,age,email,tdate)
select name,age,email,getdate() from @ex

end
GO

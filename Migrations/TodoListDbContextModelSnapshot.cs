﻿// <auto-generated />
using System;
using CollaborativeToDoList.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CollaborativeToDoList.Migrations
{
    [DbContext(typeof(TodoListDbContext))]
    partial class TodoListDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CollaborativeToDoList.Models.Categories", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("CollaborativeToDoList.Models.Collaborators", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("CanEdit")
                        .HasColumnType("bit");

                    b.Property<int?>("TodoListsId")
                        .HasColumnType("int");

                    b.Property<int?>("UsersId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TodoListsId");

                    b.HasIndex("UsersId");

                    b.ToTable("Collaborators");
                });

            modelBuilder.Entity("CollaborativeToDoList.Models.Tasks", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CategoriesId1")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("EndedAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("TodoListsId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CategoriesId1");

                    b.HasIndex("TodoListsId");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("CollaborativeToDoList.Models.TodoLists", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("SharedUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UsersId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UsersId");

                    b.ToTable("TodoLists");
                });

            modelBuilder.Entity("CollaborativeToDoList.Models.Users", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("isAdmin")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("CollaborativeToDoList.Models.Collaborators", b =>
                {
                    b.HasOne("CollaborativeToDoList.Models.TodoLists", null)
                        .WithMany("Collaborators")
                        .HasForeignKey("TodoListsId");

                    b.HasOne("CollaborativeToDoList.Models.Users", null)
                        .WithMany("Collaborators")
                        .HasForeignKey("UsersId");
                });

            modelBuilder.Entity("CollaborativeToDoList.Models.Tasks", b =>
                {
                    b.HasOne("CollaborativeToDoList.Models.Categories", null)
                        .WithMany("Tasks")
                        .HasForeignKey("CategoriesId1");

                    b.HasOne("CollaborativeToDoList.Models.TodoLists", null)
                        .WithMany("Tasks")
                        .HasForeignKey("TodoListsId");
                });

            modelBuilder.Entity("CollaborativeToDoList.Models.TodoLists", b =>
                {
                    b.HasOne("CollaborativeToDoList.Models.Users", null)
                        .WithMany("TodoLists")
                        .HasForeignKey("UsersId");
                });

            modelBuilder.Entity("CollaborativeToDoList.Models.Categories", b =>
                {
                    b.Navigation("Tasks");
                });

            modelBuilder.Entity("CollaborativeToDoList.Models.TodoLists", b =>
                {
                    b.Navigation("Collaborators");

                    b.Navigation("Tasks");
                });

            modelBuilder.Entity("CollaborativeToDoList.Models.Users", b =>
                {
                    b.Navigation("Collaborators");

                    b.Navigation("TodoLists");
                });
#pragma warning restore 612, 618
        }
    }
}

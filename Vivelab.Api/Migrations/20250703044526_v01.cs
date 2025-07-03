using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Vivelab.Api.Migrations
{
    /// <inheritdoc />
    public partial class v01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MetodoPagos",
                columns: table => new
                {
                    Codigo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PeiGoPaymentToken = table.Column<string>(type: "text", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetodoPagos", x => x.Codigo);
                });

            migrationBuilder.CreateTable(
                name: "Planes",
                columns: table => new
                {
                    Codigo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Precio = table.Column<double>(type: "double precision", nullable: false),
                    CantidadUsuarios = table.Column<int>(type: "integer", nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Planes", x => x.Codigo);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Codigo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    TipoUsuario = table.Column<string>(type: "text", nullable: false),
                    Bibliografia = table.Column<string>(type: "text", nullable: true),
                    Saldo = table.Column<double>(type: "double precision", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Codigo);
                });

            migrationBuilder.CreateTable(
                name: "Albumes",
                columns: table => new
                {
                    Codigo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PortadaUrl = table.Column<string>(type: "text", nullable: true),
                    ArtistaCodigo = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Albumes", x => x.Codigo);
                    table.ForeignKey(
                        name: "FK_Albumes_Usuarios_ArtistaCodigo",
                        column: x => x.ArtistaCodigo,
                        principalTable: "Usuarios",
                        principalColumn: "Codigo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Playlists",
                columns: table => new
                {
                    Codigo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    UsuarioCodigo = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Playlists", x => x.Codigo);
                    table.ForeignKey(
                        name: "FK_Playlists_Usuarios_UsuarioCodigo",
                        column: x => x.UsuarioCodigo,
                        principalTable: "Usuarios",
                        principalColumn: "Codigo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Suscripciones",
                columns: table => new
                {
                    Codigo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FechaInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false),
                    PlanCodigo = table.Column<int>(type: "integer", nullable: false),
                    MetodoPagoCodigo = table.Column<int>(type: "integer", nullable: false),
                    UsuarioCodigo = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suscripciones", x => x.Codigo);
                    table.ForeignKey(
                        name: "FK_Suscripciones_MetodoPagos_MetodoPagoCodigo",
                        column: x => x.MetodoPagoCodigo,
                        principalTable: "MetodoPagos",
                        principalColumn: "Codigo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Suscripciones_Planes_PlanCodigo",
                        column: x => x.PlanCodigo,
                        principalTable: "Planes",
                        principalColumn: "Codigo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Suscripciones_Usuarios_UsuarioCodigo",
                        column: x => x.UsuarioCodigo,
                        principalTable: "Usuarios",
                        principalColumn: "Codigo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Canciones",
                columns: table => new
                {
                    Codigo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Titulo = table.Column<string>(type: "text", nullable: false),
                    Duracion = table.Column<TimeSpan>(type: "interval", nullable: false),
                    ArchivoUrl = table.Column<string>(type: "text", nullable: false),
                    FechaSubida = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TotalReproducciones = table.Column<int>(type: "integer", nullable: false),
                    ArtistaCodigo = table.Column<int>(type: "integer", nullable: false),
                    AlbumCodigo = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Canciones", x => x.Codigo);
                    table.ForeignKey(
                        name: "FK_Canciones_Albumes_AlbumCodigo",
                        column: x => x.AlbumCodigo,
                        principalTable: "Albumes",
                        principalColumn: "Codigo");
                    table.ForeignKey(
                        name: "FK_Canciones_Usuarios_ArtistaCodigo",
                        column: x => x.ArtistaCodigo,
                        principalTable: "Usuarios",
                        principalColumn: "Codigo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsuariosSuscripciones",
                columns: table => new
                {
                    Codigo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SuscripcionCodigo = table.Column<int>(type: "integer", nullable: false),
                    UsuarioCodigo = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuariosSuscripciones", x => x.Codigo);
                    table.ForeignKey(
                        name: "FK_UsuariosSuscripciones_Suscripciones_SuscripcionCodigo",
                        column: x => x.SuscripcionCodigo,
                        principalTable: "Suscripciones",
                        principalColumn: "Codigo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuariosSuscripciones_Usuarios_UsuarioCodigo",
                        column: x => x.UsuarioCodigo,
                        principalTable: "Usuarios",
                        principalColumn: "Codigo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlaylistCanciones",
                columns: table => new
                {
                    Codigo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PlaylistCodigo = table.Column<int>(type: "integer", nullable: false),
                    CancionCodigo = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaylistCanciones", x => x.Codigo);
                    table.ForeignKey(
                        name: "FK_PlaylistCanciones_Canciones_CancionCodigo",
                        column: x => x.CancionCodigo,
                        principalTable: "Canciones",
                        principalColumn: "Codigo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlaylistCanciones_Playlists_PlaylistCodigo",
                        column: x => x.PlaylistCodigo,
                        principalTable: "Playlists",
                        principalColumn: "Codigo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Albumes_ArtistaCodigo",
                table: "Albumes",
                column: "ArtistaCodigo");

            migrationBuilder.CreateIndex(
                name: "IX_Canciones_AlbumCodigo",
                table: "Canciones",
                column: "AlbumCodigo");

            migrationBuilder.CreateIndex(
                name: "IX_Canciones_ArtistaCodigo",
                table: "Canciones",
                column: "ArtistaCodigo");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistCanciones_CancionCodigo",
                table: "PlaylistCanciones",
                column: "CancionCodigo");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistCanciones_PlaylistCodigo",
                table: "PlaylistCanciones",
                column: "PlaylistCodigo");

            migrationBuilder.CreateIndex(
                name: "IX_Playlists_UsuarioCodigo",
                table: "Playlists",
                column: "UsuarioCodigo");

            migrationBuilder.CreateIndex(
                name: "IX_Suscripciones_MetodoPagoCodigo",
                table: "Suscripciones",
                column: "MetodoPagoCodigo");

            migrationBuilder.CreateIndex(
                name: "IX_Suscripciones_PlanCodigo",
                table: "Suscripciones",
                column: "PlanCodigo");

            migrationBuilder.CreateIndex(
                name: "IX_Suscripciones_UsuarioCodigo",
                table: "Suscripciones",
                column: "UsuarioCodigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosSuscripciones_SuscripcionCodigo",
                table: "UsuariosSuscripciones",
                column: "SuscripcionCodigo");

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosSuscripciones_UsuarioCodigo",
                table: "UsuariosSuscripciones",
                column: "UsuarioCodigo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlaylistCanciones");

            migrationBuilder.DropTable(
                name: "UsuariosSuscripciones");

            migrationBuilder.DropTable(
                name: "Canciones");

            migrationBuilder.DropTable(
                name: "Playlists");

            migrationBuilder.DropTable(
                name: "Suscripciones");

            migrationBuilder.DropTable(
                name: "Albumes");

            migrationBuilder.DropTable(
                name: "MetodoPagos");

            migrationBuilder.DropTable(
                name: "Planes");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}

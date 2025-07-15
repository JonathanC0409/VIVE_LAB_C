let reproduccionesHoy = window.reproduccionesHoy || 0;
let planCodigo = window.planCodigo || 0;
let descargasHoy = window.descargasHoy || 0;
let limiteDescargas = window.limiteDescargas || 0;

console.log('Plan del usuario:', planCodigo);
console.log('Reproducciones hoy:', reproduccionesHoy);
console.log('Descargas hoy:', descargasHoy);
console.log('Límite de descargas:', limiteDescargas);

document.addEventListener('DOMContentLoaded', function () {
    const banner = document.getElementById("bannerAnuncio");
    let contadorAnuncios = 0;
    let anuncioCada = 3;
    let reproduciendoAnuncio = false;

    document.querySelectorAll('.audio-player').forEach(audio => {
        audio.addEventListener('play', function () {
            if (reproduciendoAnuncio) {
                console.log("Anuncio ya en curso, pausing reproducción...");
                this.pause();
                return;
            }

            if (planCodigo == 0 && reproduccionesHoy >= 10) {
                console.log("Límite de reproducciones alcanzado, pausing...");
                this.pause();
                this.currentTime = 0;
                alert("Ya alcanzaste tu límite diario de 10 reproducciones en el plan gratuito.");
                return;
            }

            if (planCodigo == 0 && contadorAnuncios % anuncioCada == 0 && contadorAnuncios !== 0) {
                console.log("Mostrando anuncio...");
                this.pause();
                banner.classList.remove("d-none");
                reproduciendoAnuncio = true;

                setTimeout(() => {
                    console.log("Anuncio terminado, reanudando reproducción...");
                    banner.classList.add("d-none");
                    reproduciendoAnuncio = false;
                    this.play();
                }, 5000);

                contadorAnuncios++;
                return;
            }

            const songId = this.getAttribute('data-id');

            fetch(`/Musica/IncrementarReproduccionUsuario`, {
                method: 'POST'
            }).then(() => {
                fetch(`https://localhost:7008/api/Canciones/${songId}/incrementar-reproduccion`, {
                    method: 'POST'
                }).catch(() => console.error("Error incrementando en API"));
                reproduccionesHoy++;
                contadorAnuncios++;
            });
        });

        audio.addEventListener('seeking', function () {
            if (reproduciendoAnuncio) {
                console.log("Intento de buscar durante anuncio, pausando...");
                this.pause();
                this.currentTime = 0;
            }
        });
    });

    document.querySelectorAll(".download-btn").forEach(btn => {
        btn.addEventListener("click", async function () {
            const id = this.getAttribute("data-id");

            if (descargasHoy >= limiteDescargas) {
                console.log("Límite de descargas alcanzado");
                alert("Has alcanzado tu límite diario de descargas.");
                this.disabled = true;
                return;
            }

            try {
                console.log("Iniciando descarga para canción con ID: " + id);
                const response = await fetch(`/Musica/Descargar?cancionId=${id}`);
                if (!response.ok) {
                    const msg = await response.text();
                    alert("Error: " + msg);
                    return;
                }

                const blob = await response.blob();
                const url = window.URL.createObjectURL(blob);
                const a = document.createElement('a');
                a.href = url;
                a.download = `cancion_${id}.mp3`;
                a.click();
                window.URL.revokeObjectURL(url);

                descargasHoy++;
                console.log(`Descargas hoy: ${descargasHoy}`);
                if (descargasHoy >= limiteDescargas) {
                    document.querySelectorAll(".download-btn").forEach(b => b.disabled = true);
                }
            } catch (err) {
                console.error(err);
                alert("Error al intentar descargar.");
            }
        });
    });
});
// wwwroot/js/player.js

document.addEventListener('DOMContentLoaded', function () {
    const reproductor = document.getElementById('reproductor');
    const audio = document.getElementById('audio-player');
    const imgPortada = document.getElementById('imagen-reproductor');
    const tituloElem = document.getElementById('titulo-reproductor');
    const barraProgreso = document.getElementById('barra-progreso');
    const duracionActual = document.getElementById('duracion-actual');
    const volControl = document.getElementById('vol-control');

    document.querySelectorAll('.boton-reproducir').forEach(btn => {
        btn.addEventListener('click', function () {
            console.log('dataset del botón:', this.dataset);

            audio.src = this.dataset.url;
            imgPortada.src = this.dataset.portada;
            tituloElem.textContent = this.dataset.titulo;

            reproductor.classList.remove('oculto');
            audio.play();
        });
    });

    audio.addEventListener('timeupdate', () => {
        if (!audio.duration) return;
        const porcentaje = (audio.currentTime / audio.duration) * 100;
        barraProgreso.value = porcentaje;

        const min = Math.floor(audio.currentTime / 60);
        const seg = Math.floor(audio.currentTime % 60).toString().padStart(2, '0');
        duracionActual.textContent = `${min}:${seg}`;
    });

    barraProgreso.addEventListener('input', () => {
        if (!audio.duration) return;
        audio.currentTime = (barraProgreso.value / 100) * audio.duration;
    });

    volControl.addEventListener('input', () => {
        audio.volume = volControl.value;
    });
});

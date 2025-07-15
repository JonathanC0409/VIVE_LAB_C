// Create animated background particles
function createBackgroundParticles() {
    const container = document.getElementById('backgroundAnimation');
    const particleCount = 50;

    for (let i = 0; i < particleCount; i++) {
        const particle = document.createElement('div');
        particle.className = 'particle';
        particle.style.left = Math.random() * 100 + '%';
        particle.style.top = Math.random() * 100 + '%';
        particle.style.animationDelay = Math.random() * 8 + 's';
        particle.style.animationDuration = (Math.random() * 10 + 5) + 's';
        container.appendChild(particle);
    }
}

// Intersection Observer for scroll animations
function initScrollAnimations() {
    const observerOptions = {
        threshold: 0.1,
        rootMargin: '0px 0px -50px 0px'
    };

    const observer = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.classList.add('visible');
            }
        });
    }, observerOptions);

    document.querySelectorAll('.scroll-animation').forEach(el => {
        observer.observe(el);
    });
}

// Add hover sound effect simulation
function addHoverEffects() {
    const hoverElements = document.querySelectorAll('.tarjeta-artista, .tarjeta-album, .tarjeta-cancion, .item-top, .boton-play');

    hoverElements.forEach(element => {
        element.addEventListener('mouseenter', () => {
            element.style.transition = 'all 0.4s cubic-bezier(0.175, 0.885, 0.32, 1.275)';
        });
    });
}

// Add click wave effect
function addClickWaveEffect() {
    const clickableElements = document.querySelectorAll('.btn-modern, .boton-play');

    clickableElements.forEach(element => {
        element.addEventListener('click', function (e) {

            const rect = element.getBoundingClientRect();
            const x = e.clientX - rect.left;
            const y = e.clientY - rect.top;

            const wave = document.createElement('div');
            wave.style.position = 'absolute';
            wave.style.left = x + 'px';
            wave.style.top = y + 'px';
            wave.style.width = '0';
            wave.style.height = '0';
            wave.style.borderRadius = '50%';
            wave.style.background = 'rgba(255, 255, 255, 0.6)';
            wave.style.transform = 'translate(-50%, -50%)';
            wave.style.animation = 'wave 0.6s ease-out';
            wave.style.pointerEvents = 'none';

            element.appendChild(wave);

            setTimeout(() => {
                wave.remove();
            }, 600);
        });
    });
}

// Add dynamic gradient animation to title
function animateTitle() {
    const title = document.querySelector('.hero-title');
    let hue = 0;

    setInterval(() => {
        hue = (hue + 1) % 360;
        title.style.background = `linear-gradient(45deg, #fff, #f0f0f0, hsl(${hue}, 70%, 50%))`;
        title.style.webkitBackgroundClip = 'text';
        title.style.webkitTextFillColor = 'transparent';
        title.style.backgroundClip = 'text';
    }, 100);
}

// Initialize all animations and effects
document.addEventListener('DOMContentLoaded', function () {
    createBackgroundParticles();
    initScrollAnimations();
    addHoverEffects();
    addClickWaveEffect();
    animateTitle();

    // Add CSS for wave animation
    const style = document.createElement('style');
    style.textContent = `
        @keyframes wave {
            0% {
                width: 0;
                height: 0;
                opacity: 1;
            }
            100% {
                width: 100px;
                height: 100px;
                opacity: 0;
            }
        }
        
        .btn-modern, .boton-play {
            position: relative;
            overflow: hidden;
        }
    `;
    document.head.appendChild(style);
});
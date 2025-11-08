// Script del chatbot para CineVerse
class CineVerseChatbot {
    constructor() {
        this.sessionId = this.generateSessionId();
        this.isOpen = false;
        this.isTyping = false;
        this.init();
    }

    generateSessionId() {
        return 'session_' + Math.random().toString(36).substr(2, 9) + '_' + Date.now();
    }

    init() {
        this.createChatWidget();
        this.attachEventListeners();
    }

    createChatWidget() {
        const chatWidget = document.createElement('div');
        chatWidget.innerHTML = `
            <!-- Botón flotante del chat -->
            <div id="chat-toggle" class="chat-toggle">
                <i class="fas fa-comments"></i>
                <span class="chat-notification">¡Pregúntame sobre películas!</span>
            </div>

            <!-- Ventana del chat -->
            <div id="chat-window" class="chat-window">
                <div class="chat-header">
                    <div class="chat-header-info">
                        <i class="fas fa-robot"></i>
                        <span>Asistente CineVerse</span>
                    </div>
                    <button id="chat-close" class="chat-close">
                        <i class="fas fa-times"></i>
                    </button>
                </div>
                
                <div id="chat-messages" class="chat-messages">
                    <div class="message bot-message">
                        <div class="message-avatar">
                            <i class="fas fa-robot"></i>
                        </div>
                        <div class="message-content">
                            ¡Hola! 👋 Soy tu asistente de CineVerse. Puedo ayudarte a encontrar películas perfectas para ti. Usa las sugerencias o escribe tu pregunta.
                        </div>
                        <div class="message-time">${this.getCurrentTime()}</div>
                    </div>
                </div>
                
                <div class="quick-suggestions">
                    <div class="suggestions-title">Sugerencias rápidas:</div>
                    <div class="suggestions-grid">
                        <button class="suggestion-btn" data-suggestion="películas de acción">🎬 Acción</button>
                        <button class="suggestion-btn" data-suggestion="películas de comedia">😂 Comedia</button>
                        <button class="suggestion-btn" data-suggestion="películas de drama">🎭 Drama</button>
                        <button class="suggestion-btn" data-suggestion="recomiéndame algo">⭐ Sorpréndeme</button>
                        <button class="suggestion-btn" data-suggestion="¿cómo funciona el sitio?">❓ Ayuda</button>
                    </div>
                </div>
                
                <div class="chat-input-area">
                    <input type="text" id="chat-input" placeholder="Escribe tu pregunta aquí..." maxlength="500">
                    <button id="chat-send">
                        <i class="fas fa-paper-plane"></i>
                    </button>
                </div>
            </div>
        `;
        
        document.body.appendChild(chatWidget);
    }

    attachEventListeners() {
        const toggle = document.getElementById('chat-toggle');
        const close = document.getElementById('chat-close');
        const input = document.getElementById('chat-input');
        const send = document.getElementById('chat-send');

        toggle.addEventListener('click', () => this.toggleChat());
        close.addEventListener('click', () => this.toggleChat());
        
        input.addEventListener('keypress', (e) => {
            if (e.key === 'Enter' && !e.shiftKey) {
                e.preventDefault();
                this.sendMessage();
            }
        });
        
        send.addEventListener('click', () => this.sendMessage());
        
        // Event listeners para sugerencias rápidas
        document.addEventListener('click', (e) => {
            if (e.target.classList.contains('suggestion-btn')) {
                const suggestion = e.target.getAttribute('data-suggestion');
                this.sendSuggestion(suggestion);
            }
        });
    }

    toggleChat() {
        const window = document.getElementById('chat-window');
        const toggle = document.getElementById('chat-toggle');
        
        this.isOpen = !this.isOpen;
        
        if (this.isOpen) {
            window.classList.add('open');
            toggle.style.display = 'none';
            document.getElementById('chat-input').focus();
        } else {
            window.classList.remove('open');
            toggle.style.display = 'flex';
        }
    }

    async sendMessage() {
        const input = document.getElementById('chat-input');
        const message = input.value.trim();
        
        if (!message || this.isTyping) return;
        
        // Agregar mensaje del usuario
        this.addMessage(message, 'user');
        input.value = '';
        
        // Mostrar indicador de escritura
        this.showTypingIndicator();
        
        try {
            const response = await fetch('/api/chatbot/message', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    message: message,
                    sessionId: this.sessionId
                })
            });
            
            const data = await response.json();
            
            // Remover indicador de escritura
            this.hideTypingIndicator();
            
            if (data.success) {
                // Agregar respuesta del bot
                this.addMessage(data.message, 'bot');
                
                // Mostrar películas recomendadas si las hay
                if (data.recommendedMovies && data.recommendedMovies.length > 0) {
                    this.showMovieRecommendations(data.recommendedMovies);
                }
            } else {
                this.addMessage('Lo siento, ocurrió un error. Inténtalo de nuevo.', 'bot');
            }
        } catch (error) {
            this.hideTypingIndicator();
            this.addMessage('No pude conectarme al servidor. Verifica tu conexión.', 'bot');
        }
    }

    addMessage(content, sender) {
        const messagesContainer = document.getElementById('chat-messages');
        const messageDiv = document.createElement('div');
        messageDiv.className = `message ${sender}-message`;
        
        const avatar = sender === 'bot' 
            ? '<i class="fas fa-robot"></i>' 
            : '<i class="fas fa-user"></i>';
        
        messageDiv.innerHTML = `
            <div class="message-avatar">${avatar}</div>
            <div class="message-content">${content}</div>
            <div class="message-time">${this.getCurrentTime()}</div>
        `;
        
        messagesContainer.appendChild(messageDiv);
        messagesContainer.scrollTop = messagesContainer.scrollHeight;
    }

    showMovieRecommendations(movies) {
        const messagesContainer = document.getElementById('chat-messages');
        const movieDiv = document.createElement('div');
        movieDiv.className = 'message bot-message movie-recommendations';
        
        let moviesHtml = '<div class="movies-grid">';
        movies.forEach(movie => {
            moviesHtml += `
                <div class="movie-card">
                    <h4>${movie.titulo}</h4>
                    <p class="movie-genre">${movie.genero}</p>
                    <p class="movie-description">${movie.descripcion.length > 100 
                        ? movie.descripcion.substring(0, 100) + '...' 
                        : movie.descripcion}</p>
                    <a href="/Peliculas/Details/${movie.id}" class="movie-link" target="_blank">
                        Ver detalles <i class="fas fa-external-link-alt"></i>
                    </a>
                </div>
            `;
        });
        moviesHtml += '</div>';
        
        movieDiv.innerHTML = `
            <div class="message-avatar">
                <i class="fas fa-robot"></i>
            </div>
            <div class="message-content">
                ${moviesHtml}
            </div>
            <div class="message-time">${this.getCurrentTime()}</div>
        `;
        
        messagesContainer.appendChild(movieDiv);
        messagesContainer.scrollTop = messagesContainer.scrollHeight;
    }

    showTypingIndicator() {
        this.isTyping = true;
        const messagesContainer = document.getElementById('chat-messages');
        const typingDiv = document.createElement('div');
        typingDiv.id = 'typing-indicator';
        typingDiv.className = 'message bot-message typing';
        typingDiv.innerHTML = `
            <div class="message-avatar">
                <i class="fas fa-robot"></i>
            </div>
            <div class="message-content">
                <div class="typing-dots">
                    <span></span>
                    <span></span>
                    <span></span>
                </div>
            </div>
        `;
        
        messagesContainer.appendChild(typingDiv);
        messagesContainer.scrollTop = messagesContainer.scrollHeight;
    }

    hideTypingIndicator() {
        this.isTyping = false;
        const typingIndicator = document.getElementById('typing-indicator');
        if (typingIndicator) {
            typingIndicator.remove();
        }
    }

    sendSuggestion(suggestion) {
        const input = document.getElementById('chat-input');
        input.value = suggestion;
        this.sendMessage();
    }

    getCurrentTime() {
        const now = new Date();
        return now.toLocaleTimeString('es-ES', { 
            hour: '2-digit', 
            minute: '2-digit' 
        });
    }
}

// Inicializar el chatbot cuando el DOM esté listo
document.addEventListener('DOMContentLoaded', function() {
    new CineVerseChatbot();
});
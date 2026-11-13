// =====================================================================
// ATELIER DO CUPCAKE — INTERAÇÕES DE FRONT-END (VANILLA JAVASCRIPT)
// =====================================================================

document.addEventListener('DOMContentLoaded', () => {
    initAddToCartAjax();
    initProductModal();
    initPaymentTabs();
    initResponsiveNav();
});

// Toast flutuante de notificação
function showToast(message, type = 'success') {
    let container = document.getElementById('toast-container');
    if (!container) {
        container = document.createElement('div');
        container.id = 'toast-container';
        container.style.cssText = `
            position: fixed;
            bottom: 24px;
            right: 24px;
            z-index: 9999;
            display: flex;
            flex-direction: column;
            gap: 10px;
        `;
        document.body.appendChild(container);
    }

    const toast = document.createElement('div');
    toast.className = `alert-banner alert-${type}`;
    toast.style.cssText = `
        min-width: 280px;
        box-shadow: 0 8px 24px rgba(0,0,0,0.15);
        animation: toastSlide 0.3s ease;
    `;
    toast.innerHTML = `<span>${message}</span>`;
    container.appendChild(toast);

    setTimeout(() => {
        toast.style.opacity = '0';
        toast.style.transition = 'opacity 0.4s ease';
        setTimeout(() => toast.remove(), 400);
    }, 3500);
}

// Adição assíncrona ao carrinho sem recarregar a tela
function initAddToCartAjax() {
    const forms = document.querySelectorAll('.form-add-to-cart');
    forms.forEach(form => {
        form.addEventListener('submit', async (e) => {
            e.preventDefault();
            const btn = form.querySelector('button[type="submit"]');
            const originalText = btn.innerHTML;

            const formData = new FormData(form);
            formData.append('isAjax', 'true');

            btn.disabled = true;
            btn.innerHTML = '<span>Adicionando...</span>';

            try {
                const response = await fetch(form.action, {
                    method: 'POST',
                    body: formData
                });

                if (response.ok) {
                    const data = await response.json();
                    if (data.sucesso) {
                        btn.innerHTML = '<span>✔ Adicionado!</span>';
                        btn.style.background = '#2E7D32';

                        // Atualiza o contador no cabeçalho
                        const badge = document.querySelector('.cart-badge');
                        if (badge) {
                            badge.textContent = data.totalItens;
                            badge.style.transform = 'scale(1.25)';
                            setTimeout(() => badge.style.transform = 'scale(1)', 250);
                        }

                        showToast(data.mensagem, 'success');

                        setTimeout(() => {
                            btn.innerHTML = originalText;
                            btn.style.background = '';
                            btn.disabled = false;
                        }, 1800);
                    } else {
                        showToast(data.mensagem || 'Erro ao adicionar.', 'danger');
                        btn.innerHTML = originalText;
                        btn.disabled = false;
                    }
                } else {
                    form.submit(); // fallback tradicional
                }
            } catch (err) {
                console.error(err);
                form.submit(); // fallback tradicional
            }
        });
    });
}

// Modal de detalhes do cupcake
function initProductModal() {
    const modal = document.getElementById('detailsModal');
    if (!modal) return;

    const closeBtn = modal.querySelector('.modal-close');
    if (closeBtn) {
        closeBtn.addEventListener('click', () => {
            modal.style.display = 'none';
        });
    }

    modal.addEventListener('click', (e) => {
        if (e.target === modal) modal.style.display = 'none';
    });

    document.querySelectorAll('.btn-view-details').forEach(btn => {
        btn.addEventListener('click', async () => {
            const id = btn.getAttribute('data-id');
            try {
                const res = await fetch(`/Home/Detalhes/${id}`);
                if (res.ok) {
                    const data = await res.json();
                    document.getElementById('modalTitle').textContent = data.nome;
                    document.getElementById('modalDesc').textContent = data.descricao;
                    document.getElementById('modalPrice').textContent = data.precoFormatado;
                    document.getElementById('modalImg').src = data.imagemUrl;
                    document.getElementById('modalInputId').value = data.id;

                    modal.style.display = 'flex';
                }
            } catch (e) {
                console.error(e);
            }
        });
    });
}

// Alternância entre PIX e Cartão de Crédito
function initPaymentTabs() {
    const tabPix = document.getElementById('tabPix');
    const tabCard = document.getElementById('tabCard');
    const boxPix = document.getElementById('boxPix');
    const boxCard = document.getElementById('boxCard');
    const inputForma = document.getElementById('inputFormaPagamento');

    if (!tabPix || !tabCard) return;

    tabPix.addEventListener('click', () => {
        tabPix.classList.add('active');
        tabCard.classList.remove('active');
        boxPix.style.display = 'block';
        boxCard.style.display = 'none';
        if (inputForma) inputForma.value = 'PIX';
    });

    tabCard.addEventListener('click', () => {
        tabCard.classList.add('active');
        tabPix.classList.remove('active');
        boxCard.style.display = 'block';
        boxPix.style.display = 'none';
        if (inputForma) inputForma.value = 'CARTAO_CREDITO';
    });
}

// Copiar chave PIX
function copiarChavePix() {
    const input = document.getElementById('chavePixInput');
    if (input) {
        input.select();
        navigator.clipboard.writeText(input.value);
        showToast('Chave PIX copiada para a área de transferência!', 'success');
    }
}

// Menu Hamburguer Responsivo para Telas Menores
function initResponsiveNav() {
    const navToggleBtn = document.getElementById('navToggleBtn');
    const navMenu = document.getElementById('navMenu');
    if (!navToggleBtn || !navMenu) return;

    navToggleBtn.addEventListener('click', (e) => {
        e.stopPropagation();
        navMenu.classList.toggle('show');
    });

    document.addEventListener('click', (e) => {
        if (!navMenu.contains(e.target) && !navToggleBtn.contains(e.target)) {
            navMenu.classList.remove('show');
        }
    });
}

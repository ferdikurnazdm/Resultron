document.documentElement.classList.add('js');

/* Scroll reveal */
const revealEls = document.querySelectorAll('.reveal');
if ('IntersectionObserver' in window) {
  const revealObserver = new IntersectionObserver((entries) => {
    entries.forEach((entry) => {
      if (!entry.isIntersecting) return;
      const el = entry.target;
      el.classList.add('is-visible');
      revealObserver.unobserve(el);
      // Giriş animasyonu bitince gecikmeyi kaldır (tilt/hover takılmasın)
      setTimeout(() => {
        el.style.transitionDelay = '';
        el.classList.add('reveal-done');
      }, 1100);
    });
  }, { threshold: 0.12 });

  revealEls.forEach((el, i) => {
    el.style.transitionDelay = `${Math.min(i * 45, 220)}ms`;
    revealObserver.observe(el);
  });
} else {
  revealEls.forEach((el) => el.classList.add('is-visible'));
}

/* Copy buttons */
function fallbackCopy(text) {
  const ta = document.createElement('textarea');
  ta.value = text;
  ta.setAttribute('readonly', '');
  ta.style.cssText = 'position:fixed;top:-1000px;opacity:0';
  document.body.appendChild(ta);
  ta.select();
  let ok = false;
  try { ok = document.execCommand('copy'); } catch { ok = false; }
  document.body.removeChild(ta);
  return ok;
}

document.querySelectorAll('.copy-btn').forEach((button) => {
  const original = button.textContent;
  let timer;
  button.addEventListener('click', async () => {
    const target = document.querySelector(button.dataset.copyTarget);
    if (!target) return;
    const value = target.innerText;
    let ok = false;
    try {
      if (navigator.clipboard && window.isSecureContext) {
        await navigator.clipboard.writeText(value);
        ok = true;
      } else {
        ok = fallbackCopy(value);
      }
    } catch {
      ok = fallbackCopy(value);
    }
    button.textContent = ok ? 'Copied' : 'Select & copy';
    clearTimeout(timer);
    timer = setTimeout(() => (button.textContent = original), 1500);
  });
});

/* Docs sidebar: aktif bölümü scroll'a göre işaretle */
const sidebarLinks = document.querySelectorAll('.docs-sidebar a[href^="#"]');
if (sidebarLinks.length && 'IntersectionObserver' in window) {
  const map = new Map();
  sidebarLinks.forEach((a) => {
    const sec = document.querySelector(a.getAttribute('href'));
    if (sec) map.set(sec, a);
  });
  const spy = new IntersectionObserver((entries) => {
    entries.forEach((entry) => {
      if (!entry.isIntersecting) return;
      sidebarLinks.forEach((a) => a.classList.remove('active'));
      map.get(entry.target).classList.add('active');
    });
  }, { rootMargin: '-25% 0px -60% 0px' });
  map.forEach((_, sec) => spy.observe(sec));
}

/* Tilt & magnetic (sadece hassas imleç + hareket azaltma kapalıyken) */
const fine = window.matchMedia('(pointer:fine)').matches;
const reduceMotion = window.matchMedia('(prefers-reduced-motion:reduce)').matches;

if (fine && !reduceMotion) {
  document.querySelectorAll('.tilt-card').forEach((card) => {
    card.addEventListener('mousemove', (event) => {
      const rect = card.getBoundingClientRect();
      const x = event.clientX - rect.left;
      const y = event.clientY - rect.top;
      const rx = ((y / rect.height) - 0.5) * -4;
      const ry = ((x / rect.width) - 0.5) * 4;
      card.style.setProperty('--mx', `${(x / rect.width) * 100}%`);
      card.style.setProperty('--my', `${(y / rect.height) * 100}%`);
      card.style.transform = `perspective(900px) rotateX(${rx}deg) rotateY(${ry}deg) translateY(-2px)`;
    });
    card.addEventListener('mouseleave', () => {
      card.style.transform = '';
    });
  });

  document.querySelectorAll('.magnetic').forEach((button) => {
    button.addEventListener('mousemove', (event) => {
      const rect = button.getBoundingClientRect();
      const x = event.clientX - rect.left - rect.width / 2;
      const y = event.clientY - rect.top - rect.height / 2;
      button.style.transform = `translate(${x * 0.08}px, ${y * 0.08}px)`;
    });
    button.addEventListener('mouseleave', () => {
      button.style.transform = '';
    });
  });
}

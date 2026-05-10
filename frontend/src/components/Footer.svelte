<footer class="footer">
  <div class="footer-content">
    <span>© {new Date().getFullYear()} Jakob Faschang</span>
    <span class="divider">•</span>
    <span>Built with Svelte + .NET</span>
  </div>
</footer>

<style>
  .footer {
    margin-top: 60px;
    padding: 24px 0;
    text-align: center;
    border-top: 1px solid rgba(255, 255, 255, 0.08);
    background: rgba(3, 7, 18, 0.4);
    backdrop-filter: blur(12px);
  }

  .footer-content {
    display: flex;
    justify-content: center;
    align-items: center;
    gap: 12px;
    color: #7c86a6;
    font-size: 14px;
    letter-spacing: 0.04em;
  }

  .footer-content span {
    transition: 0.2s;
  }

  .footer-content span:hover {
    color: #67e8f9;
  }

  .divider {
    opacity: 0.4;
  }
</style>
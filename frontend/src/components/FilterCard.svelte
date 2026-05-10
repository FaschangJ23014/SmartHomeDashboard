<script lang="ts">
  let {
    roomSearch = $bindable(),
    deviceSearch = $bindable()
  } = $props<{
    roomSearch: string;
    deviceSearch: string;
  }>();
</script>

<section class="filter-card">
  <div class="filter-info">
    <p class="filter-label">SMART FILTERS</p>
    <h3>Find rooms and devices instantly</h3>
  </div>

  <input placeholder="Search room" bind:value={roomSearch} />
  <input placeholder="Search device" bind:value={deviceSearch} />
</section>

<style>
  .filter-card {
    display: flex;
    gap: 12px;
    justify-content: flex-end;
    margin-bottom: 28px;
    padding: 16px;
    border-radius: 24px;
    background: rgba(3, 7, 18, 0.45);
    border: 1px solid rgba(255,255,255,0.12);
    backdrop-filter: blur(18px);
  }

  .filter-info {
    flex: 1;
  }

  .filter-label {
    margin: 0 0 6px;
    color: #67e8f9;
    font-size: 11px;
    letter-spacing: 0.16em;
    text-transform: uppercase;
  }

  .filter-info h3 {
    margin: 0;
    color: white;
    font-size: 22px;
  }

  input {
    min-width: 220px;
    padding: 12px 14px;
    border-radius: 16px;
    border: 1px solid rgba(255,255,255,0.18);
    background: rgba(3, 7, 18, 0.65);
    color: white;
    font-size: 15px;
    outline: none;
  }

  @media (max-width: 720px) {
    .filter-card {
      flex-direction: column;
    }

    input {
      width: 100%;
      box-sizing: border-box;
    }
  }
</style>
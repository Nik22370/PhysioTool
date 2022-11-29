<script setup>
import { RouterLink, RouterView } from 'vue-router'
</script>

<template>
  <header>
    <div class="wrapper">
      <nav>
        <div><RouterLink to="/">Home</RouterLink></div>
        <div><RouterLink to="/calendar">Calendar</RouterLink></div>
      </nav>
    </div>
  </header>
  <main>
    <RouterView />
  </main>
</template>

<style scoped>
header {
  flex: 0 0 3em;
}

nav  {
  display: flex;
  gap: 1em;
}

main {
  flex-grow: 1;
}
</style>

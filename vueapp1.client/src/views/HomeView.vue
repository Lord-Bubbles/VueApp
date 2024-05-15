<script setup>
  import SideBar from '../components/SideBar.vue';
  import { useRoute } from 'vue-router';

  const route = useRoute();
</script>

<template>
  <main class="d-flex">
    <SideBar />
    <div class="p-4 m-4 w-75">
      <router-view v-if="route.name === 'profile'" />
      <router-view v-else />
    </div>
  </main>
</template>



<script setup>
  import SideBar from '@/components/SideBar.vue';
</script>

<template>
  <div class="content-container">
    <SideBar class="sidebar-wrapper" />
    <div class="p-4 m-4 content-wrapper"><router-view /></div>
  </div>
</template>

<style scoped>
  .content-container {
    display: grid;
    grid-template-columns: 12rem auto;
    grid-template-areas: 'sidebar info';
  }

  .sidebar-wrapper {
    height: 100vh;
    background-color: black;
    grid-area: sidebar;
    position: sticky;
    top: 0;
    z-index: 1000;
  }

  .content-wrapper {
    grid-area: info;
    overflow: auto;
  }

  @media (width <= 992px) {
    .content-container {
      grid-template-columns: 5.5rem auto;
    }
  }

  @media (width <= 576px) {
    .content-container {
      grid-template-columns: 1rem auto;
    }

    .sidebar-wrapper {
      background-color: transparent;
    }
  }
</style>

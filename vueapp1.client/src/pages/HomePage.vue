<script setup>
  import SideBar from '@/components/SideBar.vue';
</script>

<template>
  <div class="d-flex">
    <SideBar />
    <div class="p-4 m-4 w-75">
      <router-view />
    </div>
  </div>
</template>



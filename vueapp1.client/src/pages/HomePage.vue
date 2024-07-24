<script setup>
  import SideBar from '@/components/SideBar.vue';
</script>

<template>
  <div class="d-flex">
    <SideBar />
    <div class="py-4 pe-4 me-4 my-4 w-75 vp-margin"><router-view /></div>
  </div>
</template>

<style scoped>
  .vp-margin {
    margin-left: 14rem;
  }

  @media (width <= 992px) {
    .vp-margin {
      margin-left: 8rem;
    }
  }

  @media (width <= 576px) {
    .vp-margin {
      margin-left: 0.2rem;
    }
  }
</style>

<script setup>
  import { useRoute } from 'vue-router';
  import { useAuthStore } from './stores/authStore';
  import { useManagerStore } from './stores/managerStore';
  import { onMounted, provide } from 'vue';

  const store = useAuthStore();
  const { updateUser, removeUser } = store;
  const route = useRoute();
  provide('user', { user: store.userProfile, updateUser, removeUser });

  const managerStore = useManagerStore();
  const { getManagers } = managerStore;

  onMounted(async () => await getManagers());

  provide('allManagers', { managerNames: managerStore.managerNames, getManagers });
</script>

<template>
  <div v-if="!store.userProfile" :class="{ 'center-div': !store.userProfile }">
    <router-view v-if="route.name === 'login'" />
    <router-view v-else />
  </div>
  <div v-else>
    <router-view />
  </div>
</template>

<style scoped>
  .center-div {
    display: flex;
    align-content: center;
    justify-content: center;
    padding-top: 15%;
    padding-bottom: 15%;
    margin: 0 auto;
  }
</style>
<script setup>
  import { useAuthStore } from '@/stores/authStore';
  import { storeToRefs } from 'pinia';
  import { useRoute } from 'vue-router';

  const route = useRoute();
  const authStore = useAuthStore();
  const { user } = storeToRefs(authStore);

  const logout = async () => {
    await authStore.logout();
  };
</script>

<template>
  <div v-if="user" class="d-flex flex-column flex-shrink-0 p-3 bg-black" style="height: 100vh">
    <ul class="nav flex-column mb-auto">
      <li class="nav-item">
        <router-link
          class="nav-link"
          :class="{
            active: route.name == 'home',
            'link-light': route.name != 'home',
            'link-info': route.name == 'home'
          }"
          :to="{ name: 'home', params: { id: user.id } }"
        >
          <i class="bi bi-house-door me-1"></i>
          Home
        </router-link>
      </li>
      <li class="nav-item">
        <router-link
          class="nav-link"
          :class="{
            active: route.name == 'profile',
            'link-light': route.name != 'profile',
            'link-info': route.name == 'profile'
          }"
          :to="{ name: 'profile', params: { id: user.id } }"
        >
          <i class="bi bi-person-circle me-1"></i>
          Profile
        </router-link>
      </li>
      <li class="nav-item">
        <router-link
          class="nav-link"
          :to="{
            name: 'performance',
            params: { id: user.id },
            query: { limit: 4, page: 1, type: 'self' }
          }"
          :class="{
            active: route.name == 'performance',
            'link-light': route.name != 'performance',
            'link-info': route.name == 'performance'
          }"
        >
          <i class="bi bi-collection-fill me-1"></i>
          Performance Reviews
        </router-link>
      </li>
      <li class="nav-item" v-if="user.accountType === 'Admin'">
        <router-link
          class="nav-link"
          :to="{ name: 'admin', params: { id: user.id }, query: { limit: 2, page: 1 } }"
          :class="{
            active: route.name == 'admin',
            'link-light': route.name != 'admin',
            'link-info': route.name == 'admin'
          }"
        >
          <i class="bi bi-people-fill me-1"></i>
          All Users
        </router-link>
      </li>
      <li v-if="user.accountType === 'Manager'" class="nav-item">
        <router-link
          class="nav-link"
          :to="{ name: 'manager', params: { id: user.id } }"
          :class="{
            active: route.name == 'manager',
            'link-light': route.name != 'manager',
            'link-info': route.name == 'manager'
          }"
        >
          <i class="bi bi-people-fill me-1"></i>
          Team Members
        </router-link>
      </li>
    </ul>
    <hr />
    <button
      type="button"
      class="text-white bg-transparent border border-0 text-start"
      id="Logout"
      @click="() => logout()"
    >
      <i class="bi bi-box-arrow-right me-1"></i>
      Logout
    </button>
  </div>
</template>

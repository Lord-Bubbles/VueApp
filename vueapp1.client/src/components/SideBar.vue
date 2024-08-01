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
  <aside v-if="user">
    <button
      type="button"
      class="text-white bg-transparent border border-0 d-flex d-sm-none btn btn-lg"
      data-bs-toggle="offcanvas"
      data-bs-target="#sidebar"
    >
      <i class="bi bi-list"></i>
    </button>
    <div
      id="sidebar"
      class="offcanvas-sm offcanvas-start bg-black d-flex flex-column flex-shrink-0"
      tabindex="-1"
    >
      <div class="offcanvas-header">
        <button
          type="button"
          class="btn-close btn-close-white"
          data-bs-dismiss="offcanvas"
          data-bs-target="#sidebar"
        ></button>
      </div>
      <div class="offcanvas-body">
        <ul class="nav">
          <li class="nav-item py-2 w-100" data-bs-dismiss="offcanvas" data-bs-target="#sidebar">
            <router-link
              class="nav-link text-start text-sm-center text-lg-start"
              :class="{
                active: route.name == 'home',
                'link-light': route.name != 'home',
                'link-info': route.name == 'home'
              }"
              :to="{ name: 'home' }"
            >
              <i class="bi bi-house-door me-2"></i>
              <span>Home</span>
            </router-link>
          </li>
          <li class="nav-item py-2 w-100" data-bs-dismiss="offcanvas" data-bs-target="#sidebar">
            <router-link
              class="nav-link text-start text-sm-center text-lg-start"
              :class="{
                active: route.name == 'profile',
                'link-light': route.name != 'profile',
                'link-info': route.name == 'profile'
              }"
              :to="{ name: 'profile' }"
            >
              <i class="bi bi-person-circle me-2"></i>
              <span>Profile</span>
            </router-link>
          </li>
          <li class="nav-item py-2 w-100" data-bs-dismiss="offcanvas" data-bs-target="#sidebar">
            <router-link
              class="nav-link text-start text-sm-center text-lg-start"
              :to="{
                name: 'performance',
                query: { limit: 20, page: 1, type: 'self' }
              }"
              :class="{
                active: route.name == 'performance',
                'link-light': route.name != 'performance',
                'link-info': route.name == 'performance'
              }"
            >
              <i class="bi bi-collection-fill me-2"></i>
              <span>Reviews</span>
            </router-link>
          </li>
          <li
            class="nav-item py-2 w-100"
            v-if="user.accountType === 'Admin'"
            data-bs-dismiss="offcanvas"
            data-bs-target="#sidebar"
          >
            <router-link
              class="nav-link text-start text-sm-center text-lg-start"
              :to="{ name: 'admin', query: { limit: 20, page: 1 } }"
              :class="{
                active: route.name == 'admin',
                'link-light': route.name != 'admin',
                'link-info': route.name == 'admin'
              }"
            >
              <i class="bi bi-people-fill me-2"></i>
              <span>Users</span>
            </router-link>
          </li>
          <li
            v-if="user.accountType === 'Manager'"
            class="nav-item py-2 w-100"
            data-bs-dismiss="offcanvas"
            data-bs-target="#sidebar"
          >
            <router-link
              class="nav-link text-start text-sm-center text-lg-start"
              :to="{ name: 'manager' }"
              :class="{
                active: route.name == 'manager',
                'link-light': route.name != 'manager',
                'link-info': route.name == 'manager'
              }"
            >
              <i class="bi bi-people-fill me-2"></i>
              <span>Team</span>
            </router-link>
          </li>
          <button
            id="logout-button"
            type="button"
            class="text-white bg-transparent border border-0 px-3 py-3"
            @click="() => logout()"
          >
            <i class="bi bi-box-arrow-right me-2"></i>
            <span>Logout</span>
          </button>
        </ul>
      </div>
    </div>
  </aside>
</template>

<style scoped>
  .link-light:hover {
    background-color: rgb(66, 65, 65);
  }

  #logout-button {
    position: absolute;
    bottom: 0;
  }

  @media (width <= 576px) {
    .offcanvas-body {
      padding: 0;
      margin: 0;
      overflow: hidden;
    }

    #sidebar {
      width: 12rem;
    }
  }
</style>

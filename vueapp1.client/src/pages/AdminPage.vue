<script setup>
  import { ref } from 'vue';
  import UserFormModal from '@/components/UserFormModal.vue';
  import SearchAndFilter from '@/components/SearchAndFilter.vue';
  import Pagination from '@/components/Pagination.vue';
  import { useRoute } from 'vue-router';
  import { keepPreviousData, useQuery } from '@tanstack/vue-query';
  import { getUsers } from '@/utils/userService';

  const mode = ref(null);
  const modal = ref(false);
  const modalData = ref({});
  const route = useRoute();

  const filterData = ref({
    name: route.query.name || '',
    minAge: route.query.minAge || 0,
    maxAge: route.query.maxAge || 0,
    managerName: route.query.managerName || '',
    type: route.query.type || '',
    email: route.query.email || '',
    page: route.query.page,
    limit: route.query.limit
  });

  const { data } = useQuery({
    queryKey: ['users', filterData.value],
    queryFn: () => getUsers(filterData.value),
    placeholderData: keepPreviousData
  });

  const handleClick = (type, user) => {
    mode.value = type;
    modalData.value = user;
    modal.value = true;
  };

  const headers = [
    'First Name',
    'Last Name',
    'Age',
    'Email',
    'Birthday',
    'Phone Number',
    'Manager',
    'Type'
  ];

  const displayData = (user, field) => {
    if (field === 'birthday') {
      return new Date(user[field]).toLocaleDateString('en-US', {
        day: '2-digit',
        month: '2-digit',
        year: 'numeric'
      });
    }
    return user[field];
  };
</script>

<template>
  <section class="container">
    <h1 class="mb-3">All Users</h1>
    <SearchAndFilter v-model="filterData" />
    <section v-if="data && data.count > 0">
      <div class="table-responsive">
        <table class="table table-striped table-hover table-bordered table-dark">
          <thead>
            <tr>
              <th v-for="field in headers" :key="field">
                {{ field }}
              </th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(user, index) of data.users" :key="user.id">
              <td v-for="field in Object.keys(user)" :key="field + '-' + index">
                {{ user[field] }}
              </td>
              <td>
                <button
                  type="button"
                  class="btn btn-primary"
                  data-bs-target="#formModal"
                  @click="handleClick('edit', user)"
                >
                  Edit
                </button>
                <button type="button" class="btn btn-danger" @click="handleClick('delete', user)">
                  Delete
                </button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
      <Pagination
        v-model:page.number="filterData.page"
        v-model:limit.number="filterData.limit"
        v-model:count.number="data.count"
      />
    </section>
    <UserFormModal v-if="modal" :mode="mode" :data="modalData" v-model="modal" />
  </section>
</template>

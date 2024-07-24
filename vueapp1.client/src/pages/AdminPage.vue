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
    maxAge: route.query.maxAge || 100,
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
    'ID',
    'First Name',
    'Last Name',
    'Age',
    'Email',
    'Birthday',
    'Phone Number',
    'Manager',
    'Type'
  ];
</script>

<template>
  <section class="w-75">
    <h1 class="mb-3">All Users</h1>
    <SearchAndFilter v-model="filterData" />
    <section v-if="data?.count > 0">
      <div class="table-responsive">
        <table class="table table-striped table-hover table-bordered table-dark">
          <thead>
            <tr>
              <th v-for="(field, index) in headers" :key="field + '-' + index">
                {{ field }}
              </th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(user, index) in data.users" :key="user.id">
              <td v-for="(value, key) in user" :key="key + '-' + index">
                {{ value }}
              </td>
              <td>
                <button
                  type="button"
                  class="btn btn-primary me-2"
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

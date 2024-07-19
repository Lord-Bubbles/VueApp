<script setup>
  import { ref } from 'vue';
  import { useRoute, useRouter } from 'vue-router';
  import Pagination from '@/components/Pagination.vue';
  import ReviewFormModal from '@/components/ReviewFormModal.vue';
  import { useAuthStore } from '@/stores/authStore';
  import { getPerformances } from '@/utils/performanceService';
  import { keepPreviousData, useQuery } from '@tanstack/vue-query';

  const authStore = useAuthStore();

  const route = useRoute();
  const router = useRouter();

  const params = ref({
    type: route.query.type,
    page: route.query.page,
    limit: route.query.limit,
    user: authStore.user.id
  });
  const modal = ref(false);

  const { data } = useQuery({
    queryKey: ['performances', params.value],
    queryFn: () => getPerformances(params.value),
    initialData: keepPreviousData
  });

  const updateQuery = (val, key) => {
    params.value[key] = val;
    const query = Object.assign({}, route.query);
    query[key] = val;
    router.replace({ query });
  };
</script>

<template>
  <div class="container-fluid">
    <div class="container-fluid d-flex justify-content-around m-4 p-4">
      <div></div>
      <div class="d-flex flex-shrink-1">
        <button
          type="button"
          class="btn btn-outline-light rounded-start-pill btn-lg"
          :class="{ active: params.type === 'self' }"
          @click="updateQuery('self', 'type')"
        >
          Self Reviews
        </button>
        <button
          type="button"
          class="btn btn-outline-light rounded-end-pill btn-lg"
          :class="{ active: params.type === 'manager' }"
          @click="() => updateQuery('manager', 'type')"
        >
          Manager Reviews
        </button>
      </div>
      <button
        class="btn btn-primary rounded-circle btn-lg"
        :class="{ invisible: params.type == 'manager' }"
        @click="() => (modal = true)"
        data-bs-toggle="tooltip"
        data-bs-placement="top"
        data-bs-title="Add Review"
      >
        <i class="bi bi-plus"></i>
      </button>
    </div>
    <section v-if="data && data.count > 0">
      <div
        class="row row-cols-lg-4 row-cols-xl-5 row-cols-xxl-6 row-cols-md-3 row-cols-1 row-cols-sm-2 g-4 mb-4"
      >
        <div class="col" v-for="(performance, index) in data.performances" :key="performance.id">
          <div class="card">
            <div class="card-header">
              {{ new Date(performance.createdAt).toLocaleString('en-US') }}
            </div>
            <div class="card-body">
              <div class="card-text mb-3">
                Rating:
                <div class="d-inline-flex flex-wrap ms-1 text-secondary">
                  <span
                    :class="{ 'text-black': n <= performance.rating }"
                    v-for="n in 5"
                    :key="performance.id + '-star-' + n"
                  >
                    &#9733;
                  </span>
                </div>
              </div>
              <div class="accordion">
                <div class="accordion-item">
                  <div class="accordion-header">
                    <button
                      class="accordion-button collapsed"
                      type="button"
                      data-bs-toggle="collapse"
                      :data-bs-target="'#performance-goals-' + index"
                    >
                      Goals
                    </button>
                  </div>
                  <div :id="'performance-goals-' + index" class="accordion-collapse collapse">
                    <div class="accordion-body">
                      <ul>
                        <li
                          v-for="(goal, idx) in performance.goals"
                          :key="performance.id + '-goals-' + idx"
                        >
                          {{ goal }}
                        </li>
                      </ul>
                    </div>
                  </div>
                </div>
                <div class="accordion-item">
                  <div class="accordion-header">
                    <button
                      class="accordion-button collapsed"
                      type="button"
                      data-bs-toggle="collapse"
                      :data-bs-target="'#things-to-improve-' + index"
                    >
                      Things to Improve
                    </button>
                  </div>
                  <div :id="'things-to-improve-' + index" class="accordion-collapse collapse">
                    <div class="accordion-body">
                      <ul>
                        <li
                          v-for="(improve, idx) in performance.improve"
                          :key="performance.id + '-improve-' + idx"
                        >
                          {{ improve }}
                        </li>
                      </ul>
                    </div>
                  </div>
                </div>
                <div class="accordion-item">
                  <div class="accordion-header">
                    <button
                      class="accordion-button collapsed"
                      type="button"
                      data-bs-toggle="collapse"
                      :data-bs-target="'#things-did-well-' + index"
                    >
                      Things did well
                    </button>
                  </div>
                  <div :id="'things-did-well-' + index" class="accordion-collapse collapse">
                    <div class="accordion-body">
                      <ul>
                        <li
                          v-for="(well, idx) in performance.well"
                          :key="performance.id + '-well-' + idx"
                        >
                          {{ well }}
                        </li>
                      </ul>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
      <Pagination
        v-model:limit.number="params.limit"
        v-model:page.number="params.page"
        v-model:count.number="data.count"
      />
    </section>
    <ReviewFormModal v-if="modal" :type="params.type" :userID="authStore.user.id" v-model="modal" />
  </div>
</template>

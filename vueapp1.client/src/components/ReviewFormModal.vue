<script setup>
  import { useMutation, useQueryClient } from '@tanstack/vue-query';
  import { createPerformance } from '@/utils/performanceService';
  import { array, number, object, string } from 'yup';
  import { FieldArray, useForm, Field, ErrorMessage } from 'vee-validate';
  import { useToast } from 'vue-toastification';

  const props = defineProps({
    type: {
      required: true,
      type: String
    },
    userID: {
      type: Number,
      required: true
    }
  });
  const queryClient = useQueryClient();

  const { mutate } = useMutation({
    mutationFn: (params) => createPerformance(params),
    onSuccess: async () => {
      modal.value = false;
      return await queryClient.invalidateQueries({
        queryKey: ['performances'],
        refetchType: 'all'
      });
    },
    onError: () => {
      const toast = useToast();
      toast.error('An unexpected error has occurred while creating performance review');
    }
  });

  const modal = defineModel({ required: true, type: Boolean });

  const schema = object().shape({
    rating: number().moreThan(0, ({ more }) => `Rating must be greater than ${more}`),
    goals: array(string().required('Goal field can not be empty')).min(
      1,
      ({ min }) => `There must be at least ${min} goal`
    ),
    improve: array(string().required('Improve field can not be empty')).min(
      1,
      ({ min }) => `There must be at least ${min} point of improvement`
    ),
    well: array(string().required('Well field can not be empty')).min(
      1,
      ({ min }) => `There must be at least ${min} point you did well`
    )
  });

  const initialValues = {
    rating: 0,
    goals: [],
    improve: [],
    well: []
  };

  const { isSubmitting, handleSubmit, values, setFieldValue } = useForm({
    validationSchema: schema,
    initialValues,
    validateOnMount: false
  });

  const addReview = handleSubmit((values) => {
    mutate({ ...values, userID: props.userID, type: props.type });
  });

  // Reverse ratings to account for hover effect
  const ratings = [5, 4, 3, 2, 1];
</script>

<template>
  <div
    class="modal fade"
    data-bs-backdrop="static"
    data-bs-keyboard="false"
    tabindex="-1"
    :class="{ show: modal, 'd-block': modal }"
  >
    <div class="modal-dialog modal-dialog-centered">
      <div class="modal-content text-light-emphasis">
        <div class="modal-header">
          <button type="button" class="btn-close" @click="() => (modal = false)"></button>
        </div>
        <div class="modal-body">
          <form @submit="addReview">
            <div class="mb-3">
              <label class="form-label me-1">Performance Rating: </label>
              <div id="rating" class="d-inline-flex flex-wrap text-secondary">
                <span
                  v-for="num in ratings"
                  :key="'star-' + num"
                  class="bg-transparent border-0 p-0"
                  :class="{ 'text-orange fw-bold': num <= values.rating }"
                  @click="setFieldValue('rating', num)"
                >
                  &#9733;
                </span>
              </div>
              <ErrorMessage class="d-block text-danger" name="rating" />
            </div>
            <div class="mb-3">
              <FieldArray name="goals" v-slot="{ fields, push, remove }">
                <div class="d-flex mb-1">
                  <label class="form-label me-2">Goals</label>
                  <button
                    type="button"
                    class="btn btn-primary rounded-circle btn-sm"
                    @click="push('')"
                  >
                    <i class="bi bi-plus"></i>
                  </button>
                </div>
                <div v-for="(field, index) in fields" :key="field.key" class="mb-1">
                  <div class="d-flex justify-content-evenly">
                    <Field type="text" class="form-control me-2" :name="`goals[${index}]`" />
                    <button
                      type="button"
                      class="btn btn-danger rounded-circle btn-sm"
                      @click="remove(index)"
                    >
                      <i class="bi bi-dash-lg"></i>
                    </button>
                  </div>
                  <ErrorMessage :name="`goals[${index}]`" class="text-danger" />
                </div>
              </FieldArray>
              <ErrorMessage name="goals" class="text-danger" />
            </div>
            <div class="mb-3">
              <FieldArray name="well" v-slot="{ fields, push, remove }">
                <div class="d-flex mb-1">
                  <label class="form-label me-2">Things Did Well</label>
                  <button
                    type="button"
                    class="btn btn-primary rounded-circle btn-sm"
                    @click="push('')"
                  >
                    <i class="bi bi-plus"></i>
                  </button>
                </div>
                <div v-for="(field, index) in fields" :key="field.key" class="mb-1">
                  <div class="d-flex justify-content-evenly">
                    <Field type="text" class="form-control me-2" :name="`well[${index}]`" />
                    <button
                      type="button"
                      class="btn btn-danger rounded-circle btn-sm"
                      @click="remove(index)"
                    >
                      <i class="bi bi-dash-lg"></i>
                    </button>
                  </div>
                  <ErrorMessage :name="`well[${index}]`" class="text-danger" />
                </div>
              </FieldArray>
              <ErrorMessage name="well" class="text-danger" />
            </div>
            <div class="mb-3">
              <FieldArray name="improve" v-slot="{ fields, push, remove }">
                <div class="d-flex mb-1">
                  <label class="form-label me-2">Things to Improve</label>
                  <button
                    type="button"
                    class="btn btn-primary rounded-circle btn-sm"
                    @click="push('')"
                  >
                    <i class="bi bi-plus"></i>
                  </button>
                </div>
                <div class="mb-1" v-for="(field, index) in fields" :key="field.key">
                  <div class="d-flex justify-content-evenly mb-1">
                    <Field type="text" class="form-control me-2" :name="`improve[${index}]`" />
                    <button
                      type="button"
                      class="btn btn-danger rounded-circle btn-sm"
                      @click="remove(index)"
                    >
                      <i class="bi bi-dash-lg"></i>
                    </button>
                  </div>
                  <ErrorMessage :name="`improve[${index}]`" class="text-danger" />
                </div>
              </FieldArray>
              <ErrorMessage name="improve" class="text-danger" />
            </div>
            <div class="d-flex justify-content-center">
              <button type="submit" class="btn btn-primary">
                {{ isSubmitting ? 'Creating...' : 'Create' }}
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
  #rating {
    cursor: pointer;
    unicode-bidi: bidi-override;
    direction: rtl;
  }

  #rating > span {
    position: relative;
  }

  #rating > span:hover::before,
  #rating span:hover ~ span::before {
    content: '\2605';
    position: absolute;
    color: orange;
  }
</style>
